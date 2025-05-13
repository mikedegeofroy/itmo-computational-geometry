using lab6.BeachLine;
using lab6.Graph;
using lab6.Interfaces;
using lab6.Services;
using lab6.Utils;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IEventQueue, EventQueue>();
services.AddSingleton<IBeachLine, BeachLine>();
services.AddSingleton<IGraph, Graph>();

services.AddSingleton<Graph>();

services.AddSingleton<ISvgRenderer, SvgRenderer>();
services.AddSingleton<IVoronoiSolver, VoronoiSolver>();

var provider = services.BuildServiceProvider();
var solver = provider.GetRequiredService<IVoronoiSolver>();
var renderer = provider.GetRequiredService<ISvgRenderer>();

var sites = new List<Point>
{
    new(5, 27),
    new(6, 11),
    new(15, 39),
    new(30, 24),
    new(1, 33),
    new(18, 36),
    new(11, 3),
    new(39, 10),
    new(18, 14),
    new(3, 38),
};

// var rand = new Random();
// var sites = new List<Point>();
//
// for (int i = 0; i < 10; i++)
// {
//     int x = rand.Next(0, 150); // inclusive lower, exclusive upper
//     int y = rand.Next(0, 150);
//     sites.Add(new Point(x, y));
// }

solver.Run(sites);

var edges = solver.GetEdges();
renderer.RenderToSvg(edges, "voronoi.svg");