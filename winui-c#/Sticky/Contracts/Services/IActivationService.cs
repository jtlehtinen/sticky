using System.Threading.Tasks;

namespace Sticky.Contracts.Services;

public interface IActivationService {
  Task ActivateAsync(object activationArgs);
}
