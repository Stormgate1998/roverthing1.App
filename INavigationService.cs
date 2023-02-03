using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roverthing1
{
    public interface INavigationService
    {
        Task NavigateToAsync(string destination);
        Task NavigateToAsync(string destination, IDictionary<string, object> parameters);
    }

    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync(string destination)
        {
            await Shell.Current.GoToAsync(destination);
        }

        public async Task NavigateToAsync(string destination, IDictionary<string, object> parameters)
        {
            await Shell.Current.GoToAsync(destination, parameters);
        }
    }
}