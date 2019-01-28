﻿using System;
using System.Diagnostics;
using System.IO;

namespace DumbPrograms.ChromeDevTools.Generator
{
    class MappingTypesGenerator : CodeGenerator
    {
        public void GenerateCode(TextWriter writer, ProtocolDescriptor protocol)
        {
            StartTextWriter(writer);

            WIL("using System;");
            WIL("using System.Collections.Generic;");
            WIL("using Newtonsoft.Json;");

            WL();

            using (WILBlock("namespace DumbPrograms.ChromeDevTools.Protocol"))
            {
                foreach (var domain in protocol.Domains)
                {
                    WILSummary(domain.Description);

                    using (WILBlock($"namespace {domain.Name}"))
                    {
                        if (domain.Types != null)
                        {
                            WL();
                            WIL("#region Types");

                            foreach (var type in domain.Types)
                            {
                                WILSummary(type.Description);

                                if (type.Deprecated)
                                {
                                    WIL("[Obsolete]");
                                }

                                switch (type.Type)
                                {
                                    case JsonTypes.Any:
                                    case JsonTypes.Boolean:
                                    case JsonTypes.Integer:
                                    case JsonTypes.Number:
                                    case JsonTypes.Array:
                                    case JsonTypes.String:
                                        var nativeType = GetCSharpType(type.Type, optional: false, type.ArrayType);
                                        WIL($"[JsonConverter(typeof(JSAliasConverter<{type.Name}, {nativeType}>))]");
                                        using (WILBlock($"public class {type.Name} : JS{(type.EnumValues != null ? "Enum" : $"Alias<{nativeType}>")}"))
                                        {
                                            if (type.EnumValues != null)
                                            {
                                                Debug.Assert(nativeType == "string");

                                                foreach (var value in type.EnumValues)
                                                {
                                                    WIL($"public static {type.Name} {GetCSharpIdentifier(value)} => New<{type.Name}>(\"{value}\");");
                                                }
                                            }
                                        }
                                        break;
                                    case JsonTypes.Object:
                                        using (WILBlock($"public class {type.Name}{(type.Properties == null ? " : Dictionary<string, object>" : "")}"))
                                        {
                                            WILProperties(type.Properties);
                                        }
                                        break;
                                    default:
                                        throw new UnreachableCodeReachedException();
                                }
                            }

                            WL();
                            WIL("#endregion");
                        }

                        if (domain.Commands != null)
                        {
                            WL();
                            WIL("#region Commands");

                            foreach (var command in domain.Commands)
                            {
                                WILSummary(command.Description);

                                if (command.Deprecated)
                                {
                                    WIL("[Obsolete]");
                                }

                                var commandClassName = GetCSharpIdentifier(command.Name);
                                var commandInterface = "ICommand";
                                if (command.Returns != null)
                                {
                                    commandInterface += $"<{commandClassName}Response>";
                                }

                                using (WILBlock($"public class {commandClassName}Command : {commandInterface}"))
                                {
                                    WIL($"string ICommand.Name {{ get; }} = \"{domain.Name}.{command.Name}\";");

                                    WILProperties(command.Parameters);
                                }

                                if (command.Returns != null)
                                {
                                    WL();
                                    using (WILBlock($"public class {commandClassName}Response"))
                                    {
                                        WILProperties(command.Returns);
                                    }
                                }
                            }

                            WL();
                            WIL("#endregion");
                        }

                        if (domain.Events != null)
                        {
                            WL();
                            WIL("#region Events");

                            foreach (var @event in domain.Events)
                            {
                                WILSummary(@event.Description);

                                if (@event.Deprecated)
                                {
                                    WIL("[Obsolete]");
                                }

                                WIL($"[Event(\"{domain.Name}.{@event.Name}\")]");

                                using (WILBlock($"public class {GetCSharpIdentifier(@event.Name)}Event"))
                                {
                                    WILProperties(@event.Parameters);
                                }
                            }

                            WL();
                            WIL("#endregion");
                        }
                    }
                }
            }

            Writer.Flush();
        }

        void WILProperties(PropertyDescriptor[] properties)
        {
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    string csPropType;
                    if (property.Type != null)
                    {
                        csPropType = GetCSharpType(property.Type.Value, property.Optional, property.ArrayType);
                    }
                    else if (property.TypeReference != null)
                    {
                        csPropType = property.TypeReference;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    WILSummary($"{(property.Optional ? "Optional. " : "")}{property.Description}");
                    WIL($"[JsonProperty(\"{property.Name}\")]");
                    WIL($"public {csPropType} {GetCSharpIdentifier(property.Name)} {{ get; set; }}");
                }
            }
        }
    }
}
