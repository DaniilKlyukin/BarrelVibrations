using BarrelVibrations.ModelingObjects.AmmoFolder;
using BasicLibraryWinForm;
using InletBallisticLibrary;

namespace BarrelVibrations.Solvers
{

    public partial class SolverIdentifier
    {
        public class PowderPowerCloner : ParametricSolverCloner
        {
            public override MainSolver GеtClone(MainSolver mainSolver, double powderForce)
            {
                var solver = new MainSolver
                {
                    Material = mainSolver.Material,
                    MuzzleBreak = mainSolver.MuzzleBreak,
                    Barrel = mainSolver.Barrel,
                    MeshProperties = mainSolver.MeshProperties,
                    Environment = mainSolver.Environment,
                    ModelProperties = mainSolver.ModelProperties,
                    IsStop = (time, missileInBarrel) => !missileInBarrel
                };

                var ammoList = new List<Ammo>();

                foreach (var ammo in mainSolver.Ammo)
                {
                    var a = new Ammo
                    {
                        Name = ammo.Name,
                        Missile = ammo.Missile,
                        PowderCharge = new PowderCharge
                        {
                            IgniterMass = ammo.PowderCharge.IgniterMass,
                            SleeveDensity = ammo.PowderCharge.SleeveDensity,
                            SleeveMass = ammo.PowderCharge.SleeveMass,
                            Powders = ammo.PowderCharge.Powders.Select(powder =>
                            {
                                var p = powder.GetCopy();
                                p.f = powderForce * 1e3;
                                return p;
                            }).ToList()
                        }
                    };

                    ammoList.Add(a);
                }

                solver.Ammo = ammoList;

                return solver;
            }
        }
    }
}
