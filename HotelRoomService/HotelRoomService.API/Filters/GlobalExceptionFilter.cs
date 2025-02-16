﻿using HotelRoomService.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HotelRoomService.API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var response = new ContentResult();

            response.Content = context.Exception.Message;
            response.StatusCode = 500;
            response.ContentType = "text/plain";

            if (context.Exception is BadRequestException)
            {
                response.Content = context.Exception.Message;
                response.StatusCode = 400;
            }
            if (context.Exception is NotFoundException)
            {
                response.Content = context.Exception.Message;
                response.StatusCode = 404;
            }
            if (context.Exception is ArgumentNullException)
            {
                response.Content = context.Exception.Message;
                response.StatusCode = 400;
            }

            context.Result = response;
            base.OnException(context);
        }
    }
}
