using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Services.InterFaces;
using Talabat.Repository;
using Talabat.Repository.Repositories;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class AddApplicationServices
    {
        public static IServiceCollection addApplicationServices(this IServiceCollection services)
        {
           services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

           services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

           services.AddScoped<IOrderService, OrderService>();

           services.AddScoped<IPaymentServices , PaymentService>();
           services.AddAutoMapper(typeof(MappingProfile));

           services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                            .SelectMany(P => P.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors

                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            return services;
        }
    }
}
