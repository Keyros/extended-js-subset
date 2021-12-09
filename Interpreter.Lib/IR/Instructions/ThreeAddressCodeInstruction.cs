using System;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class ThreeAddressCodeInstruction : Instruction
    {
        public string Left { get; set; }

        protected readonly (IValue left, IValue right) right;
        private readonly string @operator;

        public ThreeAddressCodeInstruction(
            string left,
            (IValue left, IValue right) right,
            string @operator,
            int number
        ) :
            base(number)
        {
            Left = left;
            this.right = right;
            this.@operator = @operator;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = frames.Peek();
            if (right.left == null)
            {
                var value = right.right.Get(frame);
                frame[Left] = @operator switch
                {
                    "-" => -Convert.ToDouble(value),
                    "!" => !Convert.ToBoolean(value),
                    "" => value,
                    _ => frame[Left]
                };
            }
            else
            {
                object lValue = right.left.Get(frame), rValue = right.right.Get(frame);
                if (lValue is string)
                {
                    frame[Left] = lValue.ToString() + rValue;
                }
                else
                {
                    frame[Left] = Convert.ToDouble(lValue) + Convert.ToDouble(rValue);
                }
                frame[Left] = @operator switch
                {
                    "+" when lValue is string => lValue.ToString() + rValue,
                    "+" => Convert.ToDouble(lValue) + Convert.ToDouble(rValue),
                    "-" => Convert.ToDouble(lValue) - Convert.ToDouble(rValue),
                    "*" => Convert.ToDouble(lValue) * Convert.ToDouble(rValue),
                    "/" => Convert.ToDouble(lValue) / Convert.ToDouble(rValue),
                    "%" => Convert.ToDouble(lValue) % Convert.ToDouble(rValue),
                    "||" => Convert.ToBoolean(lValue) || Convert.ToBoolean(rValue),
                    "&&" => Convert.ToBoolean(lValue) && Convert.ToBoolean(rValue),
                    ">" => Convert.ToDouble(lValue) > Convert.ToDouble(rValue),
                    ">=" => Convert.ToDouble(lValue) >= Convert.ToDouble(rValue),
                    "<" => Convert.ToDouble(lValue) < Convert.ToDouble(rValue),
                    "<=" => Convert.ToDouble(lValue) <= Convert.ToDouble(rValue),
                    _ => frame[Left]
                };
            }

            return Jump();
        }

        protected override string ToStringRepresentation() =>
            right.left == null
                ? $"{Left} = {(@operator == "" ? "" : " " + @operator)}{right.right}"
                : $"{Left} = {right.left} {@operator} {right.right}";
    }
}