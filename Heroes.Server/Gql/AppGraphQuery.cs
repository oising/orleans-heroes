﻿using GraphQL;
using GraphQL.Types;
using Heroes.Contracts.Heroes;
using Heroes.Contracts.Stats;
using Heroes.Server.Gql.Types;
using Heroes.Server.Sample;

namespace Heroes.Server.Gql
{
	public class AppGraphQuery : ObjectGraphType
	{
		public AppGraphQuery(
			IHeroGrainClient heroGrainClient,
			IHeroStatsGrainClient heroStatsClient,
			IHeroService mockHeroService
		)
		{
			Name = "AppQueries";

			Field<HeroGraphType>(
				name: "hero",
				description: "hero full object",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "key", Description = "Unique key for specific hero" }
					),
				resolve: context =>
				{
					var result = heroGrainClient.Get(context.GetArgument<string>("key"));
					return result;
				}
			);

			Field<ListGraphType<HeroGraphType>>(
				name: "heroes",
				description: "heroes list",
				arguments: new QueryArguments(
					new QueryArgument<HeroRoleGraphType> { Name = "role", Description = "filtering heroes by role." }
				),
				resolve: context =>
				{
					var role = context.GetArgument<int?>("role");
					var result = heroGrainClient.GetAll((HeroRoleType?)role);
					return result;
				}
			);

			Field<HeroStatsGraphType>(
				name: "herostats",
				description: "view all hero stats",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "key", Description = "Unique key for specific hero" }
				),
				resolve: context =>
				{
					var result = heroStatsClient.Get(context.GetArgument<string>("key"));
					return result;
				}
			);


			Field<ListGraphType<HeroGraphType>>(
				name: "heroesMock",
				description: "heroes list",
				resolve: context =>
				{
					var result = mockHeroService.Heroes();
					return result;
				}
			);
		}
	}
}
