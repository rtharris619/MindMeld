using Application.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class QuoteMapper
    {
        public static QuoteDTO MapToQuoteDTO(this Quote quoteModel)
        {
            return new QuoteDTO
            {
                Id = quoteModel.Id,
                Description = quoteModel.Description,
                CreatedDate = quoteModel.CreatedDate,
                ModifiedDate = quoteModel.ModifiedDate,
                Author = quoteModel.Author?.Name,                
            };
        }

        public static Quote MapToQuoteModel(this QuoteDTO quoteDTO)
        {
            return new Quote
            {
                Id = quoteDTO.Id,
                Description = quoteDTO.Description,
                CreatedDate = quoteDTO.CreatedDate,
                ModifiedDate = quoteDTO.ModifiedDate,
                Author = new Author { Name = quoteDTO.Author }
            };
        }

        public static List<QuoteDTO> MapToQuoteDTOs(this List<Quote>? quoteModels)
        {
            var quoteDTOs = new List<QuoteDTO>();

            if (quoteModels == null)
            {
                return quoteDTOs;
            }

            foreach (var quoteModel in quoteModels)
            {
                quoteDTOs.Add(MapToQuoteDTO(quoteModel));
            }

            return quoteDTOs;
        }

        public static List<Quote> MapToQuoteModels(this List<QuoteDTO> quoteDTOs)
        {
            var quoteModels = new List<Quote>();

            foreach (var quoteDTO in quoteDTOs)
            {
                quoteModels.Add(MapToQuoteModel(quoteDTO));
            }

            return quoteModels;
        }
    }
}
