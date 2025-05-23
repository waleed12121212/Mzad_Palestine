﻿using AutoMapper;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.DTO_s.AutoBid;
using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.DTO_s.Customer_Support;
using Mzad_Palestine_Core.DTO_s.Dispute;
using Mzad_Palestine_Core.DTO_s.Invoice;
using Mzad_Palestine_Core.DTO_s.Message;
using Mzad_Palestine_Core.DTO_s.Notification;
using Mzad_Palestine_Core.DTO_s.Payment;
using Mzad_Palestine_Core.DTO_s.Review;
using Mzad_Palestine_Core.DTO_s.Subscription;
using Mzad_Palestine_Core.DTO_s.Tag;
using Mzad_Palestine_Core.DTO_s.User;
using Mzad_Palestine_Core.DTO_s.Watchlist;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.DTOs.Listing;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mzad_Palestine_Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile( )
        {
            // User mapping
            CreateMap<User , UserDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.Id));
            CreateMap<RegisterUserDto , User>();
            // No need to ignore Id as it will be set by Identity framework

            // Listing mapping
            CreateMap<Listing, ListingDto>()
                .ForMember(dest => dest.ListingId, opt => opt.MapFrom(src => src.ListingId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images != null ? src.Images.Select(i => i.ImageUrl).ToList() : new List<string>()));
            CreateMap<CreateListingDto, Listing>();

            // Auction mapping
            CreateMap<Auction , AuctionDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.AuctionId))
                .ForMember(dest => dest.WinnerId, opt => opt.MapFrom(src => src.WinnerId))
                .ForMember(dest => dest.CategoryName , opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.Images , opt => opt.MapFrom(src => src.Images != null ? src.Images.Select(i => i.ImageUrl).ToList() : null));
            CreateMap<CreateAuctionDto , Auction>();

            // Bid mapping
            CreateMap<Bid , Mzad_Palestine_Core.DTO_s.Auction.BidDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.BidId))
                .ForMember(dest => dest.UserName , opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.IsWinningBid , opt => opt.MapFrom(src => src.Auction != null && src.Auction.WinnerId == src.UserId));
            CreateMap<CreateBidDto , Bid>();

            // Payment mapping
            CreateMap<Payment , PaymentDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.PaymentId));
            CreateMap<CreatePaymentDto , Payment>();

            // Invoice mapping
            CreateMap<Invoice , InvoiceDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.InvoiceId));

            // Message mapping
            CreateMap<Message , MessageDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.MessageId));
            CreateMap<CreateMessageDto , Message>();

            // Review mapping
            CreateMap<Review , ReviewDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.ReviewedUserId));
            CreateMap<CreateReviewDto , Review>();

            // Report mapping
            CreateMap<Report , ReportDto>()
                .ForMember(dest => dest.ReportId , opt => opt.MapFrom(src => src.ReportId))
                .ForMember(dest => dest.ReporterName , opt => opt.MapFrom(src => src.Reporter != null ? src.Reporter.UserName : null))
                .ForMember(dest => dest.ReportedListingTitle , opt => opt.MapFrom(src => src.ReportedListing != null ? src.ReportedListing.Title : null))
                .ForMember(dest => dest.ResolverName , opt => opt.MapFrom(src => src.Resolver != null ? src.Resolver.UserName : null));
            CreateMap<CreateReportDto , Report>();

            // Notification mapping
            CreateMap<Notification , NotificationDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.NotificationId));

            // AutoBid mapping
            CreateMap<AutoBid , AutoBidDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.AutoBidId));
            CreateMap<CreateAutoBidDto , AutoBid>();

            // Dispute mapping
            CreateMap<Dispute , DisputeDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.DisputeId));
            CreateMap<CreateDisputeDto , Dispute>();

            // Tag mapping
            CreateMap<Tag , TagDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.TagId));

            // ListingTag mapping
            CreateMap<ListingTag , ListingTagDto>();

            // Watchlist mapping
            CreateMap<Watchlist , WatchlistDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.WatchlistId));

            // Subscription mapping
            CreateMap<Subscription , SubscriptionDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.SubscriptionId));

            // Support Ticket mapping
            CreateMap<CustomerSupportTicket , SupportTicketDto>()
                .ForMember(dest => dest.Id , opt => opt.MapFrom(src => src.TicketId));  // Changed from TicketId to Id
            CreateMap<CreateSupportTicketDto , CustomerSupportTicket>();
        }
    }
}