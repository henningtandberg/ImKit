using System;
using ImKit.DockSpace;
using ImKit.MonoGame.Rendering;
using ImKit.MonoGame.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImKit.MonoGame;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the MonoGame ImGui renderer and an <see cref="IGui"/> host that draws the
    /// registered <see cref="IDockSpace"/> (if any) followed by every registered
    /// <see cref="IGuiWidget"/>.
    /// </summary>
    /// <remarks>
    /// The <c>GraphicsDevice</c> and <c>GameWindow</c> are supplied as factories because
    /// neither exists until <c>Game.Initialize</c> has run, which is typically after the
    /// container is built. They are resolved lazily, on first use of the renderer.
    /// </remarks>
    public static IServiceCollection AddImKit(
        this IServiceCollection services,
        Func<IServiceProvider, GraphicsDevice> graphicsDeviceFactory,
        Func<IServiceProvider, GameWindow> gameWindowFactory) =>
        services
            .AddSingleton<IWrapper<GraphicsDevice>>(sp => new GraphicsDeviceWrapper(graphicsDeviceFactory(sp)))
            .AddSingleton<IWrapper<GameWindow>>(sp => new GameWindowWrapper(gameWindowFactory(sp)))
            .AddSingleton<ImGuiRenderer>()
            .AddSingleton<IImGuiRenderer>(sp => sp.GetRequiredService<ImGuiRenderer>())
            .AddSingleton<IGuiBackend>(sp => sp.GetRequiredService<ImGuiRenderer>())
            .AddSingleton<IGui>(sp => new GuiHost(
                sp.GetRequiredService<IGuiBackend>(),
                sp.GetServices<IGuiWidget>(),
                sp.GetService<IDockSpace>()));

    /// <summary>
    /// Registers a widget both as its concrete type and as <see cref="IGuiWidget"/>, backed
    /// by a single instance. Registering the concrete type too lets the widget be injected
    /// directly, or resolved by other roles it implements — an event subscriber, say —
    /// without a second instance being created.
    /// </summary>
    /// <remarks>
    /// The concrete registration uses <c>TryAdd</c>, so an existing registration (from a
    /// messaging framework that scanned the assembly, for example) wins and the widget that
    /// draws stays the same instance that receives events.
    /// </remarks>
    public static IServiceCollection AddGuiWidget<TWidget>(this IServiceCollection services)
        where TWidget : class, IGuiWidget
    {
        services.TryAddSingleton<TWidget>();
        return services.AddSingleton<IGuiWidget>(sp => sp.GetRequiredService<TWidget>());
    }

    /// <summary>
    /// Registers a dock space both as its concrete type and as <see cref="IDockSpace"/>,
    /// backed by a single instance. The concrete registration uses <c>TryAdd</c>, so an
    /// existing registration wins.
    /// </summary>
    public static IServiceCollection AddDockSpace<TDockSpace>(this IServiceCollection services)
        where TDockSpace : class, IDockSpace
    {
        services.TryAddSingleton<TDockSpace>();
        return services.AddSingleton<IDockSpace>(sp => sp.GetRequiredService<TDockSpace>());
    }
}
