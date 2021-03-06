﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NRules.RuleModel.Builders
{
    /// <summary>
    /// Builder to compose a binding element that associates a pattern with a calculated expression.
    /// </summary>
    public class BindingBuilder : PatternSourceElementBuilder, IBuilder<BindingElement>
    {
        private readonly Type _valueType;
        private LambdaExpression _expression;

        internal BindingBuilder(SymbolTable scope, Type valueType) : base(scope)
        {
            _valueType = valueType;
        }

        /// <summary>
        /// Adds a calculated expression to the binding element.
        /// </summary>
        /// <param name="expression">Expression to bind.</param>
        public void BindingExpression(LambdaExpression expression)
        {
            _expression = expression;
        }

        BindingElement IBuilder<BindingElement>.Build()
        {
            if (_expression == null)
                throw new ArgumentException($"BINDING element requires a binding expression.");

            IEnumerable<Declaration> references = _expression.Parameters.Select(p => Scope.Lookup(p.Name, p.Type));
            var element = new BindingElement(_valueType, Scope.VisibleDeclarations, references, _expression);
            return element;
        }
    }
}