using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StopWatchApp.Tests
{
	[TestClass()]
	public class ActionCommandTests
	{

		[TestMethod]
		public void CanExecuteOverloadExecutesTruePredicate()
		{
			var command = new ActionCommand(p => { }, p => (int)p == 1);
			Assert.IsTrue(command.CanExecute(1));
		}

		[TestMethod()]
		public void CanExecuteOverloadExecutesFalsePredicate()
		{
			var command = new ActionCommand(p => { }, p => (int)p == 1);
			Assert.IsFalse(command.CanExecute(0));
		}

		[TestMethod]
		public void CanExecuteIsTrueByDefault()
		{
			var command = new ActionCommand(p => { });
			Assert.IsTrue(command.CanExecute(null));
		}

		[TestMethod]
		public void ExecuteOverloadInvokesActionWithParameter()
		{
			var invoked = false;
			void action(object obj)
			{
				Assert.IsNotNull(obj);
				invoked = true;
			}

			var command = new ActionCommand(action);
			command.Execute(new object());

			Assert.IsTrue(invoked);
		}

		[TestMethod]
		public void ExecuteInvokesAction()
		{
			var invoked = false;

			void action(object obj) => invoked = true;

			var command = new ActionCommand(action);
			command.Execute();

			Assert.IsTrue(invoked);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorThrowsExceptionIfActionParameterIsNull()
		{
			_ = new ActionCommand(null);
		}
	}
}