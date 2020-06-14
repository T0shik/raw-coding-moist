using System;
using Moist.Core.Models;

namespace Moist.Core.Schemas
{
    public static class SchemaFactory
    {
        public static BaseSchema Resolve(Schema schema) =>
            schema.Type switch
            {
                SchemaType.DaysVisited => new DaysVisitedSchemaSchema
                {
                    Id = schema.Id,
                    Title = schema.Title,
                    Description = schema.Description,
                    State = schema.State,
                    Perpetual = schema.Perpetual,
                    Goal = schema.Goal,
                    ValidSince = schema.ValidSince,
                    ValidUntil = schema.ValidUntil
                },
                //todo add exception
                _ => throw new Exception()
            };
    }
}