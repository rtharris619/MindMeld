﻿using Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>
{

}
