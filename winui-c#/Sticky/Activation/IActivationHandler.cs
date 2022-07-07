using System.Threading.Tasks;

namespace Sticky.Activation;

public interface IActivationHandler {
  bool CanHandle(object args);

  Task HandleAsync(object args);
}
