using System;
using System.Linq;
using Mono.Cecil;

namespace Reactive.Fody
{
	public class ModuleWeaver
	{
		public ModuleDefinition ModuleDefinition { get; set; }

		public Action<string> LogInfo { get; set; }
		public Action<string> LogError { get; set; }

		public void Execute()
		{
			LogInfo("... Reactive.Fody ...");
			LogInfo("");

			if (GotDependencies()) 
			{
				var propertyWeaver = new ReactiveUIPropertyWeaver
				{
					ModuleDefinition = ModuleDefinition,
					LogInfo = LogInfo,
					LogError = LogError
				};
				propertyWeaver.Execute();

				var observableAsPropertyWeaver = new ObservableAsPropertyWeaver
				{
					ModuleDefinition = ModuleDefinition,
					LogInfo = LogInfo
				};
				observableAsPropertyWeaver.Execute();

				var reactiveDependencyWeaver = new ReactiveDependencyPropertyWeaver
				{
					ModuleDefinition = ModuleDefinition,
					LogInfo = LogInfo,
					LogError = LogError
				};
				reactiveDependencyWeaver.Execute();
			}

			LogInfo("");
			LogInfo("... Finished ...");
		}

		bool GotDependencies()
		{
			var reactiveUI = ModuleDefinition.AssemblyReferences.Where(x => x.Name == "ReactiveUI").OrderByDescending(x => x.Version).FirstOrDefault();
			if (reactiveUI == null)
			{
				LogInfo("Could not find assembly: ReactiveUI (" + string.Join(", ", ModuleDefinition.AssemblyReferences.Select(x => x.Name)) + ")");
				return false;
			}
			LogInfo($"{reactiveUI.Name} {reactiveUI.Version}");

			var helpers = ModuleDefinition.AssemblyReferences.Where(x => x.Name == "Reactive.Fody.Helpers").OrderByDescending(x => x.Version).FirstOrDefault();
			if (helpers == null)
			{
				LogInfo("Could not find assembly: Reactive.Fody.Helpers (" + string.Join(", ", ModuleDefinition.AssemblyReferences.Select(x => x.Name)) + ")");
				return false;
			}
			LogInfo($"{helpers.Name} {helpers.Version}");

			return true;
		}

	}
}
