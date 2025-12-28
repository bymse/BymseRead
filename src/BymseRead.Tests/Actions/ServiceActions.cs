using BymseRead.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BymseRead.Tests.Actions;

[AutoRegistration(Lifetime = ServiceLifetime.Singleton)]
public record ServiceActions(UsersActions Users, FilesActions Files, BooksActions Books);