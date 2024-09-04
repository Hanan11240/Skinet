using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T :BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecification<T> spec){
        if(spec.Criteria is not null){
            query = query.Where(spec.Criteria);
        }
        if(spec.OrderBy is not null){
            query = query.OrderBy(spec.OrderBy);
        }
        if(spec.OrderByDesc is not null){
            query = query.OrderByDescending(spec.OrderByDesc);
        }
        return query;
    }
}
