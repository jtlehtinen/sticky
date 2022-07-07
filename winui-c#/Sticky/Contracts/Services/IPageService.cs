using System;

namespace Sticky.Contracts.Services;

public interface IPageService {
  Type GetPageType(string key);
}
