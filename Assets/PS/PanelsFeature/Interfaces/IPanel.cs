using System.Threading.Tasks;

namespace PS.PanelsFeature.Interfaces
{
    public interface IPanel
    {
        Task Open();
        Task Close();
    }
}