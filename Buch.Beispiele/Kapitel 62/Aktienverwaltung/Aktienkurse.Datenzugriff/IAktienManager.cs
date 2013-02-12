using System.Collections.Generic;
using System.Threading.Tasks;
using Aktienkurse.DataService.Model;

namespace Aktienkurse.DataService
{
    public interface IAktienManager
    {
        Task<List<Aktie>> GetAktienBySymbole(params string[] symbole);
        Task<Aktie> GetAktieBySymbol(string symbol);
    }
}