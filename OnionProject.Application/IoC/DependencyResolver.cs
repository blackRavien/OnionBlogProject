using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using OnionProject.Application.AutoMapper;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Application.Services.ConcreteManagers;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Infrastructure.ConcreteRepositories;
using Module = Autofac.Module;

namespace OnionProject.Application.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorRepo>().As<IAuthorRepo>().InstancePerLifetimeScope();
            builder.RegisterType<GenreRepo>().As<IGenreRepo>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserRepo>().As<IAppUserRepo>().InstancePerLifetimeScope();
            builder.RegisterType<PostRepo>().As<IPostRepo>().InstancePerLifetimeScope();
            builder.RegisterType<CommentRepo>().As<ICommentRepo>().InstancePerLifetimeScope();

            builder.RegisterType<AuthorManager>().As<IAuthorService>().InstancePerLifetimeScope();
            builder.RegisterType<PostManager>().As<IPostService>().InstancePerLifetimeScope();
            builder.RegisterType<GenreManager>().As<IGenreService>().InstancePerLifetimeScope();
            builder.RegisterType<CommentManager>().As<ICommentService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactMessageRepo>().As<IContactMessageRepo>().InstancePerLifetimeScope();
            builder.RegisterType<ContactMessageManager>().As<IContactMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserManager>().As<IAppUserService>().InstancePerLifetimeScope();


            //diğer servisler ve managerlar buraya eklenecek...
            //builder.RegisterType<AppUserManager>().As<IAppUserService>().InstancePerLifetimeScope();
            //varsa başka......





            builder.Register(context => new MapperConfiguration(config =>
                    //register mapper profile
                    config.AddProfile<Mapping>()
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = c.Resolve<MapperConfiguration>();

                return config.CreateMapper(context.Resolve);
           
            }).As<IMapper>().InstancePerLifetimeScope();

            //silmiyoruz en altta kalsın.
            base.Load(builder);
        }
    }
}
