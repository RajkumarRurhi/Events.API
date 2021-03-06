// <auto-generated />
using System;
using Events.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Events.API.Migrations
{
    [DbContext(typeof(EventsDBContext))]
    partial class EventsDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Events.API.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11ce940f-0997-463a-b850-6d5a866a07a7"),
                            Description = "Raj's birthday celebration description. Raj's birthday celebration description. Raj's birthday celebration description.",
                            EndDateTime = new DateTime(2022, 1, 18, 11, 18, 10, 333, DateTimeKind.Local).AddTicks(6452),
                            EventType = "Indoor",
                            StartDateTime = new DateTime(2022, 1, 18, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(5628),
                            Title = "Raj's birthday belebration"
                        },
                        new
                        {
                            Id = new Guid("21ce940f-0997-463a-b850-6d5a866a07a7"),
                            Description = "David's marriage anniversary celebration. David's marriage anniversary celebration. David's marriage anniversary celebration.",
                            EndDateTime = new DateTime(2022, 1, 28, 13, 18, 10, 333, DateTimeKind.Local).AddTicks(7102),
                            EventType = "Outdoor",
                            StartDateTime = new DateTime(2022, 1, 28, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(7076),
                            Title = "David's marriage anniversary celebration"
                        },
                        new
                        {
                            Id = new Guid("31ce940f-0997-463a-b850-6d5a866a07a7"),
                            Description = "Georg's work anniversary celebration. Georg's work anniversary celebration. Georg's work anniversary celebration.",
                            EndDateTime = new DateTime(2022, 1, 21, 12, 18, 10, 333, DateTimeKind.Local).AddTicks(7138),
                            EventType = "Indoor",
                            StartDateTime = new DateTime(2022, 1, 21, 9, 18, 10, 333, DateTimeKind.Local).AddTicks(7133),
                            Title = "Georg's work anniversary celebration"
                        });
                });

            modelBuilder.Entity("Events.API.Entities.Person", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Email");

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            Email = "RajRurhi@events.com",
                            DateOfBirth = new DateTime(1987, 1, 11, 9, 18, 10, 325, DateTimeKind.Local).AddTicks(2342),
                            FirstName = "Raj",
                            LastName = "Rurhi"
                        },
                        new
                        {
                            Email = "DavidJones@events.com",
                            DateOfBirth = new DateTime(1972, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5723),
                            FirstName = "David",
                            LastName = "Jones"
                        },
                        new
                        {
                            Email = "GeorgeJensen@events.com",
                            DateOfBirth = new DateTime(1922, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5804),
                            FirstName = "Georg",
                            LastName = "Jensen"
                        },
                        new
                        {
                            Email = "AmitKumar@events.com",
                            DateOfBirth = new DateTime(1994, 1, 11, 9, 18, 10, 331, DateTimeKind.Local).AddTicks(5814),
                            FirstName = "Amit",
                            LastName = "Kumar"
                        });
                });

            modelBuilder.Entity("Events.API.Entities.PersonEvent", b =>
                {
                    b.Property<string>("PersonEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PersonEmail", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("PersonEvents");

                    b.HasData(
                        new
                        {
                            PersonEmail = "RajRurhi@events.com",
                            EventId = new Guid("11ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "DavidJones@events.com",
                            EventId = new Guid("11ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "GeorgeJensen@events.com",
                            EventId = new Guid("11ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "DavidJones@events.com",
                            EventId = new Guid("21ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "GeorgeJensen@events.com",
                            EventId = new Guid("21ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "AmitKumar@events.com",
                            EventId = new Guid("21ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "GeorgeJensen@events.com",
                            EventId = new Guid("31ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "AmitKumar@events.com",
                            EventId = new Guid("31ce940f-0997-463a-b850-6d5a866a07a7")
                        },
                        new
                        {
                            PersonEmail = "RajRurhi@events.com",
                            EventId = new Guid("31ce940f-0997-463a-b850-6d5a866a07a7")
                        });
                });

            modelBuilder.Entity("Events.API.Entities.PersonEvent", b =>
                {
                    b.HasOne("Events.API.Entities.Event", "Event")
                        .WithMany("Guests")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Events.API.Entities.Person", "Person")
                        .WithMany("EventsToAttend")
                        .HasForeignKey("PersonEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
