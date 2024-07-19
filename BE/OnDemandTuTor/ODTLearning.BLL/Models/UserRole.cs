using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTLearning.BLL.Models
{
    public enum UserRole
    {
        [Description("học sinh")]
        HocSinh,

        [Description("gia sư")]
        GiaSu,

        [Description("quản trị viên")]
        QuanTriVien,

        [Description("kiểm duyệt viên")]
        KiemDuyetVien
    }

    public static class UserRoleAuthorize
    {
        public const string Student = "học sinh";
        public const string Tutor = "gia sư";
        public const string Admin = "quản trị viên";
        public const string Moderator = "kiểm duyệt viên";
    }
}