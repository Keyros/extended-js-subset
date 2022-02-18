using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizables;
using Interpreter.Lib.VM;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class OptimizableTests
    {
        private IOptimizable<Instruction> optimizable;

        [Fact]
        public void IdentityExpressionTests()
        {
            optimizable = new IdentityExpression(
                new ThreeAddressCodeInstruction(
                    "", (new Constant(0, "0"), new Name("x")), "+", 0
                )
            );
            
            Assert.True(optimizable.Test());
            
            optimizable = new IdentityExpression(
                new ThreeAddressCodeInstruction(
                    "", (new Constant(1, "1"), new Name("x")), "*", 0
                )
            );
            
            Assert.True(optimizable.Test());
            
            optimizable = new IdentityExpression(
                new ThreeAddressCodeInstruction(
                    "", (new Constant(2, "2"), new Name("x")), "+", 0
                )
            );
            
            Assert.False(optimizable.Test());
        }
    }
}