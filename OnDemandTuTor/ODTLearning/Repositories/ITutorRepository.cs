﻿using Microsoft.EntityFrameworkCore;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {
        bool ConFirmProfileTutor(string idTutor, string status);
        bool UpdateTutorProfile(string idTutor, TutorProfileToConfirmModel model);
    }
}