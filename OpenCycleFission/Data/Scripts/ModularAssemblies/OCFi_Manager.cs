﻿using System.Collections.Generic;
using System.Linq;
using Sandbox.ModAPI;
using Scripts.ModularAssemblies.Communication;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Scripts.ModularAssemblies
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    internal class OCFiManager : MySessionComponentBase
    {
        public static OCFiManager I;
        public ModularDefinitionApi ModularApi = new ModularDefinitionApi();
        public Dictionary<int, OCFi_ReactorLogic> Reactors = new Dictionary<int, OCFi_ReactorLogic>();

        private bool _didRegisterAssemblyClose = false;

        public override void LoadData()
        {
            ModularApi.Init(ModContext);
            I = this;
            MyAPIGateway.Utilities.ShowNotification("OCFiManager loaded", 1000 / 60);
        }

        protected override void UnloadData()
        {
            I = null;
            MyAPIGateway.Utilities.ShowNotification("OCFiManager unloaded", 1000 / 60);
        }

        public override void UpdateAfterSimulation()
        {
            if (!ModularApi.IsReady)
            {
                MyAPIGateway.Utilities.ShowNotification("Modular API not ready", 1000 / 60);
                return;
            }

            if (!_didRegisterAssemblyClose)
            {
                ModularApi.AddOnAssemblyClose(assemblyId => Reactors.Remove(assemblyId));
                _didRegisterAssemblyClose = true;
                MyAPIGateway.Utilities.ShowNotification("Registered assembly close handler", 1000 / 60);
            }

            foreach (var reactor in Reactors.Values)
                reactor.UpdateAfterSimulation();

            var systems = ModularApi.GetAllAssemblies();
            foreach (var reactor in Reactors.Values.ToList())
                if (!systems.Contains(reactor.PhysicalAssemblyId))
                    Reactors.Remove(reactor.PhysicalAssemblyId);
        }

        public void OnPartAdd(int PhysicalAssemblyId, IMyCubeBlock NewBlockEntity, bool IsBaseBlock)
        {
            if (!Reactors.ContainsKey(PhysicalAssemblyId))
                Reactors.Add(PhysicalAssemblyId, new OCFi_ReactorLogic(PhysicalAssemblyId));

            Reactors[PhysicalAssemblyId].AddPart(NewBlockEntity);
            MyAPIGateway.Utilities.ShowNotification($"Part added to assembly {PhysicalAssemblyId}", 1000 / 60);
        }

        public void OnPartRemove(int PhysicalAssemblyId, IMyCubeBlock BlockEntity, bool IsBaseBlock)
        {
            if (!Reactors.ContainsKey(PhysicalAssemblyId))
                return;

            Reactors[PhysicalAssemblyId].RemovePart(BlockEntity);
            MyAPIGateway.Utilities.ShowNotification($"Part removed from assembly {PhysicalAssemblyId}", 1000 / 60);
        }
    }
}