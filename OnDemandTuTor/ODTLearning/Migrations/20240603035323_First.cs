using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ODTLearning.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FisrtName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gmail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__ADA956212E0C4E76", x => x.id_Account);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    id_Field = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FieldName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Field__0CC408078A47A724", x => x.id_Field);
                });

            migrationBuilder.CreateTable(
                name: "Type of service",
                columns: table => new
                {
                    id_TypeOfService = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    NameService = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Type of __0DF9946FAF2D937A", x => x.id_TypeOfService);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id_feedback = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Service = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__36BC863086749877", x => x.id_feedback);
                    table.ForeignKey(
                        name: "FK__Feedback__id_Acc__3DB3258D",
                        column: x => x.id_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id_Chat = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Messages__316B3F44B536C3B4", x => x.id_Chat);
                    table.ForeignKey(
                        name: "FK__Messages__id_Acc__54968AE5",
                        column: x => x.id_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    JwtId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: true),
                    IssuedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ID_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefreshT__3214EC0750897F01", x => x.Id);
                    table.ForeignKey(
                        name: "FK__RefreshTo__ID_Ac__5A4F643B",
                        column: x => x.ID_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                });

            migrationBuilder.CreateTable(
                name: "Tutor",
                columns: table => new
                {
                    id_Tutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SpecializedSkills = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Experience = table.Column<int>(type: "int", nullable: true),
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tutor__93DA661D916EF89E", x => x.id_Tutor);
                    table.ForeignKey(
                        name: "FK__Tutor__id_Accoun__2F650636",
                        column: x => x.id_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    id_Post = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Titile = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_TypeOfService = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Post__2BA425F7AC64A41D", x => x.id_Post);
                    table.ForeignKey(
                        name: "FK__Post__id_Account__4924D839",
                        column: x => x.id_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                    table.ForeignKey(
                        name: "FK__Post__id_TypeOfS__4A18FC72",
                        column: x => x.id_TypeOfService,
                        principalTable: "Type of service",
                        principalColumn: "id_TypeOfService");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    id_Service = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_TypeOfService = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_feedback = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__F6F54EA7038DCAB7", x => x.id_Service);
                    table.ForeignKey(
                        name: "FK__Service__id_Type__4183B671",
                        column: x => x.id_TypeOfService,
                        principalTable: "Type of service",
                        principalColumn: "id_TypeOfService");
                    table.ForeignKey(
                        name: "FK__Service__id_feed__4277DAAA",
                        column: x => x.id_feedback,
                        principalTable: "Feedback",
                        principalColumn: "id_feedback");
                });

            migrationBuilder.CreateTable(
                name: "EducationalQualifications",
                columns: table => new
                {
                    id_EducationalEualifications = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    id_Tutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CertificateName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Organization = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    img = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Educatio__C1B293B0C50E68E8", x => x.id_EducationalEualifications);
                    table.ForeignKey(
                        name: "FK__Education__id_Tu__324172E1",
                        column: x => x.id_Tutor,
                        principalTable: "Tutor",
                        principalColumn: "id_Tutor");
                });

            migrationBuilder.CreateTable(
                name: "Tutor_Field",
                columns: table => new
                {
                    id_Tutor_Fileld = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    id_Tutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Field = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tutor_Fi__CBB4B60C26DD204F", x => x.id_Tutor_Fileld);
                    table.ForeignKey(
                        name: "FK__Tutor_Fie__id_Fi__37FA4C37",
                        column: x => x.id_Field,
                        principalTable: "Field",
                        principalColumn: "id_Field");
                    table.ForeignKey(
                        name: "FK__Tutor_Fie__id_Tu__370627FE",
                        column: x => x.id_Tutor,
                        principalTable: "Tutor",
                        principalColumn: "id_Tutor");
                });

            migrationBuilder.CreateTable(
                name: "ResquestTutor",
                columns: table => new
                {
                    id_RequestTutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Tutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Post = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Resquest__61285A5E8EB2641D", x => x.id_RequestTutor);
                    table.ForeignKey(
                        name: "FK__ResquestT__id_Po__51BA1E3A",
                        column: x => x.id_Post,
                        principalTable: "Post",
                        principalColumn: "id_Post");
                    table.ForeignKey(
                        name: "FK__ResquestT__id_Tu__50C5FA01",
                        column: x => x.id_Tutor,
                        principalTable: "Tutor",
                        principalColumn: "id_Tutor");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    id_Schedule = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    TimeStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    TimeEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    id_Tutor = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Service = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__B70A1A1150945D1A", x => x.id_Schedule);
                    table.ForeignKey(
                        name: "FK__Schedule__id_Ser__46486B8E",
                        column: x => x.id_Service,
                        principalTable: "Service",
                        principalColumn: "id_Service");
                    table.ForeignKey(
                        name: "FK__Schedule__id_Tut__45544755",
                        column: x => x.id_Tutor,
                        principalTable: "Tutor",
                        principalColumn: "id_Tutor");
                });

            migrationBuilder.CreateTable(
                name: "Rent",
                columns: table => new
                {
                    id_Rent = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    id_Account = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    id_Schedule = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rent__478EEDDFA4D65269", x => x.id_Rent);
                    table.ForeignKey(
                        name: "FK__Rent__id_Account__4CF5691D",
                        column: x => x.id_Account,
                        principalTable: "Account",
                        principalColumn: "id_Account");
                    table.ForeignKey(
                        name: "FK__Rent__id_Schedul__4DE98D56",
                        column: x => x.id_Schedule,
                        principalTable: "Schedule",
                        principalColumn: "id_Schedule");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalQualifications_id_Tutor",
                table: "EducationalQualifications",
                column: "id_Tutor");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_id_Account",
                table: "Feedback",
                column: "id_Account");

            migrationBuilder.CreateIndex(
                name: "UQ__Feedback__F6F54EA6E556053E",
                table: "Feedback",
                column: "id_Service",
                unique: true,
                filter: "[id_Service] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_id_Account",
                table: "Messages",
                column: "id_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Post_id_Account",
                table: "Post",
                column: "id_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Post_id_TypeOfService",
                table: "Post",
                column: "id_TypeOfService");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ID_Account",
                table: "RefreshToken",
                column: "ID_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Rent_id_Account",
                table: "Rent",
                column: "id_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Rent_id_Schedule",
                table: "Rent",
                column: "id_Schedule");

            migrationBuilder.CreateIndex(
                name: "IX_ResquestTutor_id_Post",
                table: "ResquestTutor",
                column: "id_Post");

            migrationBuilder.CreateIndex(
                name: "IX_ResquestTutor_id_Tutor",
                table: "ResquestTutor",
                column: "id_Tutor");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_id_Service",
                table: "Schedule",
                column: "id_Service");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_id_Tutor",
                table: "Schedule",
                column: "id_Tutor");

            migrationBuilder.CreateIndex(
                name: "IX_Service_id_TypeOfService",
                table: "Service",
                column: "id_TypeOfService");

            migrationBuilder.CreateIndex(
                name: "UQ__Service__36BC8631BD63F430",
                table: "Service",
                column: "id_feedback",
                unique: true,
                filter: "[id_feedback] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_id_Account",
                table: "Tutor",
                column: "id_Account");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_Field_id_Field",
                table: "Tutor_Field",
                column: "id_Field");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_Field_id_Tutor",
                table: "Tutor_Field",
                column: "id_Tutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalQualifications");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Rent");

            migrationBuilder.DropTable(
                name: "ResquestTutor");

            migrationBuilder.DropTable(
                name: "Tutor_Field");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Tutor");

            migrationBuilder.DropTable(
                name: "Type of service");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
