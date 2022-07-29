﻿using System.Collections.Generic;
using AutoMapper;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    public class LexerQueryModel
    {
        [JsonProperty] private List<TokenTypeModel> TokenTypes { get; set; }

        public LexerQueryModel()
        {
            TokenTypes = JsonConvert.DeserializeObject<List<TokenTypeModel>>(
                Interpreter.TokenTypes.Json
            );
        }

        public string Text { get; set; }

        public Structure GetDomain(IMapper mapper) =>
            new(mapper.Map<List<TokenType>>(TokenTypes));
    }
}