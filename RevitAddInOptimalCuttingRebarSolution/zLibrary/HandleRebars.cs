using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Text;
using System.Threading.Tasks;
using RevitAddInOptimalCuttingRebarSolution.ViewModel;

namespace RevitAddInOptimalCuttingRebarSolution.zLibrary
{
    public class HandleRebars
    {
        #region Thuoc tinh data
        public List<RebarInformation> DataRebarHandeled { get; set; }
        public List<FreshRebar> DataRebarInput { get; set; }
        public double RatioWastageMass { get; set; }

        #endregion

        public HandleRebars(List<Steel> steels, SettingViewModel settingViewModel, List<RebarInformation> inputDataRebars, Document doc)
        {
            #region Xu li du lieu dau vao
            List<ElementGA> listElementGAs = new List<ElementGA>();
            List<string> IDs = inputDataRebars.DistinctBy(x => x.IDElementContain).Select(x => x.IDElementContain).ToList();
            foreach (string id in IDs)
            {
                string nameElementGA = inputDataRebars.Where(x => x.IDElementContain == id).Select(x => x.ElementContainInformation).ToList()[0];
                List<Rebar_CayThep> elementGAListRebar = new List<Rebar_CayThep>();
                foreach (RebarInformation rebarInformation in inputDataRebars)
                {
                    if (rebarInformation.IDElementContain == id)
                    {
                        Steel steel = steels.Where(x => x.Diameter == rebarInformation.Diameter).ToList()[0];
                        Rebar_CayThep rebar = new Rebar_CayThep(rebarInformation.IDRebar, rebarInformation.Lenght, steel);
                        elementGAListRebar.Add(rebar);
                    }
                }
                ElementGA elementGA = new ElementGA(id, id, elementGAListRebar);//set name cua element GA = id cua element contain
                listElementGAs.Add(elementGA);
            }
            #endregion

            #region Xu li thep
            //1. Khoi tao quan the
            Population population = new Population(settingViewModel.PopulationSize, settingViewModel.MaxGenerationQuantity, listElementGAs, steels);
            population.RatioHybridizeElement = settingViewModel.RatioHybridizeElement;
            population.RatioMutateElement = settingViewModel.RatioMutateElement;
            population.RatioMutateLocal = settingViewModel.RatioMutateLocal;
            population.FactorMutateElement = settingViewModel.FactorMutateElement;
            population.FactorMutateLocal = settingViewModel.FactorMutateLocal;
            population.RatioMutateElementLocal = settingViewModel.RatioMutateElementLocal;
            //2. Tien hoa quan the
            population.Evolution();
            #endregion

            //Lay data thep nguyen lieu
            DataRebarInput = new List<FreshRebar>();
            foreach (FreshRebar freshRebar in population.BestIndividual.ListInputRebar)
            {
                DataRebarInput.Add(freshRebar);
            }
            //Lay data thep 

            #region Lay du lieu out put
            //truong hop chua ve dc thep: lay ve id cua cac thanh thep roi sap xep lai trinh tu
            DataRebarHandeled = new List<RebarInformation>();
            List<Rebar_CayThep> ListRebar = population.BestIndividual.ToListThep();
            foreach (Rebar_CayThep thep in ListRebar)
            {
                RebarInformation rebarInformation = inputDataRebars.Where(x => x.IDRebar == thep.Name).ToList()[0];
                DataRebarHandeled.Add(rebarInformation);
            }

            RatioWastageMass = population.BestIndividual.RatioWasteMass;
            #endregion 
        }
    }
}
