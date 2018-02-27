﻿using AutoMapper;
using PMS.Models;
using PMS.Resources;
using PMS.Resources.SubResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API Resource
            CreateMap<Announcement, AnnouncementResource>()
                .ForMember(cr => cr.AnnouncementUsers, opt => opt.MapFrom(c => c.AnnouncementUsers.Select(cf => cf.AnnouncementUserId)));

            CreateMap<AnnouncementUser, AnnouncementUserResource>()
            .ForMember(cr => cr.AnnouncementId, opt => opt.MapFrom(c => c.Announcement.AnnouncementId))
            .ForMember(cr => cr.AppUser, opt => opt.MapFrom(c => new ApplicationUserResource
            {
                Avatar = c.AppUser.Avatar,
                CreatedOn = c.AppUser.CreatedOn,
                FullName = c.AppUser.FullName,
                IsDeleted = c.AppUser.IsDeleted,
                Major = c.AppUser.Major,
                UpdatedOn = c.AppUser.UpdatedOn,
                AnnouncementUsers = c.AppUser.AnnouncementUsers.Select(a => a.AnnouncementUserId).ToList()
            }));


            CreateMap<Council, CouncilResource>()
            //.ForMember(cr => cr.Group, opt => opt.MapFrom(c => c.Group))
            .ForMember(cr => cr.ProjectName, opt => opt.MapFrom(c => c.Group.Project.Title))
            .ForMember(cr => cr.GroupName, opt => opt.MapFrom(c => c.Group.GroupName))
            .ForMember(cr => cr.GroupId, opt => opt.MapFrom(c => c.Group.GroupId))
            .ForMember(cr => cr.LecturerInformations, opt => opt.MapFrom(c => new LecturerInformationResource
            {
                President = new PresidentResource
                {
                    LecturerId = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "President").Lecturer.LecturerId,
                    ScorePercent = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "President").Percentage,
                    Score = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "President").Score
                },
                Secretary = new SecretaryResource
                {
                    LecturerId = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Secretary").Lecturer.LecturerId,
                    ScorePercent = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Secretary").Percentage,
                    Score = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Secretary").Score
                },
                Reviewer = new ReviewerResource
                {
                    LecturerId = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Reviewer").Lecturer.LecturerId,
                    ScorePercent = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Reviewer").Percentage,
                    Score = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Reviewer").Score
                },
                Supervisor = new SupervisorResource
                {
                    LecturerId = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Supervisor").Lecturer.LecturerId,
                    ScorePercent = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Supervisor").Percentage,
                    Score = c.CouncilEnrollments.FirstOrDefault(cf => cf.CouncilRole.CouncilRoleName == "Supervisor").Score
                }
            }))
            .ForMember(cr => cr.CouncilEnrollments, opt => opt.MapFrom(c => c.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId)));

            CreateMap<Major, MajorResource>()
                .ForMember(gr => gr.Lecturers, opt => opt.MapFrom(g => g.Lecturers.Select(gf => gf.LecturerId)))
                .ForMember(gr => gr.Students, opt => opt.MapFrom(g => g.Students.Select(gf => gf.Id)))
                .ForMember(gr => gr.Groups, opt => opt.MapFrom(g => g.Groups.Select(gf => gf.GroupId)))
                .ForMember(gr => gr.Projects, opt => opt.MapFrom(g => g.Projects.Select(gf => gf.ProjectId)));

            CreateMap<Quarter, QuarterResource>()
                .ForMember(sr => sr.Groups, opt => opt.MapFrom(s => s.Groups.Select(sf => sf.GroupId)))
                .ForMember(sr => sr.Enrollments, opt => opt.MapFrom(s => s.Enrollments.Select(sf => sf.EnrollmentId)));


            CreateMap<CouncilEnrollment, CouncilEnrollmentResource>()
                .ForMember(cr => cr.LecturerID, opt => opt.MapFrom(c => c.Lecturer.LecturerId))
                .ForMember(cr => cr.Lecturer, opt => opt.MapFrom(c => new LecturerResource
                {
                    LecturerId = c.Lecturer.LecturerId,
                    Name = c.Lecturer.Name,
                    Address = c.Lecturer.Address,
                    DateOfBirth = c.Lecturer.DateOfBirth,
                    Email = c.Lecturer.Email,
                    IsDeleted = c.Lecturer.IsDeleted,
                    PhoneNumber = c.Lecturer.PhoneNumber,
                    CouncilEnrollments = c.Lecturer.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId).ToList(),
                    Groups = c.Lecturer.Groups.Select(cf => cf.GroupId).ToList(),
                    Projects = c.Lecturer.Projects.Select(cf => cf.ProjectId).ToList()

                }))
                .ForMember(cr => cr.CouncilID, opt => opt.MapFrom(c => c.Council.CouncilId))
                .ForMember(cr => cr.Council, opt => opt.MapFrom(c => new CouncilResource
                {
                    CouncilId = c.Council.CouncilId,
                    ResultGrade = c.Council.ResultGrade,
                    ResultScore = c.Council.ResultScore,
                    IsDeleted = c.Council.IsDeleted,
                    isAllScored = c.Council.isAllScored,
                    CouncilEnrollments = c.Council.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId).ToList()
                }));

            CreateMap<Grade, GradeResource>()
               .ForMember(gr => gr.Enrollment, opt => opt.MapFrom(g => g.Enrollment));

            CreateMap<Student, StudentResource>()
                .ForMember(sr => sr.MajorId, opt => opt.MapFrom(s => s.Major.MajorId))
                .ForMember(sr => sr.Major, opt => opt.MapFrom(s => new MajorResource
                {
                    MajorId = s.Major.MajorId,
                    MajorName = s.Major.MajorName,
                    MajorCode = s.Major.MajorCode,
                    isDeleted = s.Major.isDeleted,
                    Groups = s.Major.Groups.Select(sf => sf.GroupId).ToList(),
                    Students = s.Major.Students.Select(sf => sf.Id).ToList(),
                    Projects = s.Major.Projects.Select(sf => sf.ProjectId).ToList(),
                    Lecturers = s.Major.Lecturers.Select(sf => sf.LecturerId).ToList(),
                }))
                .ForMember(sr => sr.Enrollments, opt => opt.MapFrom(v => v.Enrollments.Select(e => e.EnrollmentId)));

            CreateMap<Enrollment, EnrollmentResource>()
                .ForMember(er => er.StudentEmail, opt => opt.MapFrom(e => e.Student.Email))
                .ForMember(er => er.StudentId, opt => opt.MapFrom(e => e.Student.Id))
                .ForMember(er => er.Grade, opt => opt.MapFrom(e => e.Grade))
                //.ForMember(er => er.Student, opt => opt.MapFrom(e => new StudentResource
                //{
                //    Id = e.Student.Id,
                //    Address = e.Student.Address,
                //    DateOfBirth = e.Student.DateOfBirth,
                //    Email = e.Student.Email,
                //    IsDeleted = e.Student.IsDeleted,
                //    Name = e.Student.Name,
                //    PhoneNumber = e.Student.PhoneNumber,
                //    Year = e.Student.Year,
                //    StudentCode = e.Student.StudentCode,
                //    Enrollments = e.Student.Enrollments.Select(ef => ef.EnrollmentId).ToList(),
                //    Major = new MajorResource
                //    {
                //        MajorId = e.Student.Major.MajorId,
                //        MajorCode = e.Student.Major.MajorCode,
                //        MajorName = e.Student.Major.MajorName,
                //        isDeleted = e.Student.Major.isDeleted,
                //        Groups = e.Student.Major.Groups.Select(sf => sf.GroupId).ToList(),
                //        Students = e.Student.Major.Students.Select(sf => sf.Id).ToList(),
                //        Projects = e.Student.Major.Projects.Select(sf => sf.ProjectId).ToList(),
                //        Lecturers = e.Student.Major.Lecturers.Select(sf => sf.LecturerId).ToList(),
                //    }
                //}))
                .ForMember(er => er.QuarterId, opt => opt.MapFrom(e => e.Quarter.QuarterId))
                .ForMember(er => er.Quarter, opt => opt.MapFrom(e => new Quarter
                {
                    QuarterId = e.Quarter.QuarterId,
                    QuarterName = e.Quarter.QuarterName,
                    QuarterEnd = e.Quarter.QuarterEnd,
                    QuarterStart = e.Quarter.QuarterStart,
                    isDeleted = e.Quarter.isDeleted
                }))
                .ForMember(er => er.GroupId, opt => opt.MapFrom(e => e.Group.GroupId))
                .ForMember(er => er.Group, opt => opt.MapFrom(e => new GroupResource
                {
                    GroupId = e.Group.GroupId,
                    GroupName = e.Group.GroupName,
                    LinkGitHub = e.Group.LinkGitHub,
                    isDeleted = e.Group.isDeleted,
                    isConfirm = e.Group.isConfirm,
                    ResultGrade = e.Group.ResultGrade,
                    ResultScore = e.Group.ResultScore,
                    Enrollments = e.Group.Enrollments.Select(ef => ef.EnrollmentId).ToList(),
                    UploadedFiles = e.Group.UploadedFiles.Select(ef => ef.UploadedFileId).ToList()
                }))
                .ForMember(er => er.LecturerId, opt => opt.MapFrom(e => e.Lecturer.LecturerId))
                .ForMember(er => er.Lecturer, opt => opt.MapFrom(e => new LecturerResource
                {
                    LecturerId = e.Lecturer.LecturerId,
                    Name = e.Lecturer.Name,
                    Address = e.Lecturer.Address,
                    DateOfBirth = e.Lecturer.DateOfBirth,
                    Email = e.Lecturer.Email,
                    IsDeleted = e.Lecturer.IsDeleted,
                    PhoneNumber = e.Lecturer.PhoneNumber,
                    CouncilEnrollments = e.Lecturer.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId).ToList(),
                    Groups = e.Lecturer.Groups.Select(cf => cf.GroupId).ToList(),
                    Projects = e.Lecturer.Projects.Select(cf => cf.ProjectId).ToList()
                }));

            CreateMap<ApplicationRole, RoleResource>();

            CreateMap<Project, ProjectResource>()
                .ForMember(pr => pr.MajorId, opt => opt.MapFrom(p => p.Major.MajorId))
                .ForMember(pr => pr.Major, opt => opt.MapFrom(p => new MajorResource
                {
                    MajorId = p.Major.MajorId,
                    MajorName = p.Major.MajorName,
                    MajorCode = p.Major.MajorCode,
                    isDeleted = p.Major.isDeleted,
                    Groups = p.Major.Groups.Select(sf => sf.GroupId).ToList(),
                    Students = p.Major.Students.Select(sf => sf.Id).ToList(),
                    Projects = p.Major.Projects.Select(sf => sf.ProjectId).ToList(),
                    Lecturers = p.Major.Lecturers.Select(sf => sf.LecturerId).ToList(),
                }))
                .ForMember(pr => pr.LecturerId, opt => opt.MapFrom(p => p.Lecturer.LecturerId))
                .ForMember(pr => pr.Lecturer, opt => opt.MapFrom(p => new LecturerResource
                {
                    LecturerId = p.Lecturer.LecturerId,
                    Name = p.Lecturer.Name,
                    Address = p.Lecturer.Address,
                    DateOfBirth = p.Lecturer.DateOfBirth,
                    Email = p.Lecturer.Email,
                    IsDeleted = p.Lecturer.IsDeleted,
                    PhoneNumber = p.Lecturer.PhoneNumber,
                    CouncilEnrollments = p.Lecturer.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId).ToList(),
                    Groups = p.Lecturer.Groups.Select(cf => cf.GroupId).ToList(),
                    Projects = p.Lecturer.Projects.Select(cf => cf.ProjectId).ToList()
                }))
                .ForMember(pr => pr.Groups, opt => opt.MapFrom(p => p.Groups.Select(pf => pf.GroupId)));

            CreateMap<Lecturer, LecturerResource>()
                .ForMember(lr => lr.MajorId, opt => opt.MapFrom(l => l.Major.MajorId))
                .ForMember(lr => lr.Major, opt => opt.MapFrom(l => new MajorResource
                {
                    MajorId = l.Major.MajorId,
                    MajorName = l.Major.MajorName,
                    MajorCode = l.Major.MajorCode,
                    isDeleted = l.Major.isDeleted,
                    Groups = l.Major.Groups.Select(sf => sf.GroupId).ToList(),
                    Students = l.Major.Students.Select(sf => sf.Id).ToList(),
                    Projects = l.Major.Projects.Select(sf => sf.ProjectId).ToList(),
                    Lecturers = l.Major.Lecturers.Select(sf => sf.LecturerId).ToList(),
                }))
                .ForMember(lr => lr.Groups, opt => opt.MapFrom(l => l.Groups.Select(lf => lf.GroupId)))
                .ForMember(lr => lr.Projects, opt => opt.MapFrom(l => l.Projects.Select(lf => lf.ProjectId)))
                .ForMember(lr => lr.CouncilEnrollments, opt => opt.MapFrom(l => l.CouncilEnrollments.Select(lf => lf.CouncilEnrollmentId)));

            CreateMap<Group, GroupResource>()
                .ForMember(gr => gr.Enrollments, opt => opt.MapFrom(g => g.Enrollments.Select(gf => gf.EnrollmentId)))
                .ForMember(gr => gr.UploadedFiles, opt => opt.MapFrom(g => g.UploadedFiles.Select(gf => gf.UploadedFileId)))
                .ForMember(er => er.QuarterId, opt => opt.MapFrom(e => e.Quarter.QuarterId))
                .ForMember(er => er.Quarter, opt => opt.MapFrom(e => new QuarterResource
                {
                    QuarterId = e.Quarter.QuarterId,
                    QuarterName = e.Quarter.QuarterName,
                    QuarterEnd = e.Quarter.QuarterEnd,
                    QuarterStart = e.Quarter.QuarterStart,
                    isDeleted = e.Quarter.isDeleted,
                    Groups = e.Quarter.Groups.Select(gf => gf.GroupId).ToList(),
                    Enrollments = e.Quarter.Enrollments.Select(ef => ef.EnrollmentId).ToList()
                }))
                .ForMember(gr => gr.MajorId, opt => opt.MapFrom(g => g.Major.MajorId))
                .ForMember(gr => gr.Major, opt => opt.MapFrom(g => new MajorResource
                {
                    MajorId = g.Major.MajorId,
                    MajorName = g.Major.MajorName,
                    MajorCode = g.Major.MajorCode,
                    isDeleted = g.Major.isDeleted,
                    Groups = g.Major.Groups.Select(sf => sf.GroupId).ToList(),
                    Students = g.Major.Students.Select(sf => sf.Id).ToList(),
                    Projects = g.Major.Projects.Select(sf => sf.ProjectId).ToList(),
                    Lecturers = g.Major.Lecturers.Select(sf => sf.LecturerId).ToList(),
                }))
                .ForMember(gr => gr.LecturerId, opt => opt.MapFrom(g => g.Lecturer.LecturerId))
                .ForMember(gr => gr.Lecturer, opt => opt.MapFrom(g => new LecturerResource
                {
                    LecturerId = g.Lecturer.LecturerId,
                    Name = g.Lecturer.Name,
                    Address = g.Lecturer.Address,
                    DateOfBirth = g.Lecturer.DateOfBirth,
                    Email = g.Lecturer.Email,
                    IsDeleted = g.Lecturer.IsDeleted,
                    PhoneNumber = g.Lecturer.PhoneNumber,
                    CouncilEnrollments = g.Lecturer.CouncilEnrollments.Select(cf => cf.CouncilEnrollmentId).ToList(),
                    Groups = g.Lecturer.Groups.Select(cf => cf.GroupId).ToList(),
                    Projects = g.Lecturer.Projects.Select(cf => cf.ProjectId).ToList()
                }))
                .ForMember(gr => gr.ProjectId, opt => opt.MapFrom(g => g.Project.ProjectId))
                .ForMember(gr => gr.Project, opt => opt.MapFrom(g => new ProjectResource
                {
                    ProjectId = g.Project.ProjectId,
                    Description = g.Project.Description,
                    ProjectCode = g.Project.ProjectCode,
                    Title = g.Project.Title,
                    Type = g.Project.Type,
                    IsDeleted = g.Project.IsDeleted,
                    IsCompleted = g.Project.IsCompleted,
                    Groups = g.Project.Groups.Select(gf => gf.GroupId).ToList()
                }));
            CreateMap<Excel, ExcelResource>();
            CreateMap<UploadedFile, UploadedFileResource>()
                .ForMember(cr => cr.GroupId, opt => opt.MapFrom(c => c.Group.GroupId))
                .ForMember(sr => sr.Group, opt => opt.MapFrom(s => new GroupResource
                {
                    GroupId = s.Group.GroupId,
                    GroupName = s.Group.GroupName,
                    LinkGitHub = s.Group.LinkGitHub,
                    isDeleted = s.Group.isDeleted,
                    isConfirm = s.Group.isConfirm,
                    ResultGrade = s.Group.ResultGrade,
                    ResultScore = s.Group.ResultScore,
                    Enrollments = s.Group.Enrollments.Select(sf => sf.EnrollmentId).ToList(),
                    UploadedFiles = s.Group.UploadedFiles.Select(sf => sf.UploadedFileId).ToList()
                }));
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));

            //API Resource to domain
            CreateMap<QueryResource, Query>();

            CreateMap<StudentResource, Student>()
            .ForMember(s => s.Id, opt => opt.Ignore())
            .ForMember(s => s.Enrollments, opt => opt.Ignore());
            //.AfterMap((sr, s) =>
            //{
            //    //remove unselected enrollments
            //    var removedEnrollments = s.Enrollments.Where(e => !sr.Enrollments.Contains(e.EnrollmentId));
            //    foreach (var e in removedEnrollments)
            //    {
            //        s.Enrollments.Remove(e);
            //    }

            //    //add new enrollments
            //    var addedEnrollments = sr.Enrollments.Where(id => !s.Enrollments.Any(e => e.EnrollmentId == id)).Select(id => new Enrollment { EnrollmentId = id });
            //    foreach (var e in addedEnrollments)
            //    {
            //        s.Enrollments.Add(e);
            //    }
            //});

            CreateMap<AnnouncementResource, Announcement>()
                .ForMember(c => c.AnnouncementId, opt => opt.Ignore());

            CreateMap<AnnouncementUserResource, AnnouncementUser>()
                .ForMember(c => c.AnnouncementUserId, opt => opt.Ignore());

            CreateMap<CouncilResource, Council>()
                .ForMember(c => c.CouncilId, opt => opt.Ignore());

            CreateMap<CouncilEnrollmentResource, CouncilEnrollment>()
                .ForMember(c => c.CouncilEnrollmentId, opt => opt.Ignore());

            CreateMap<GradeResource, Grade>()
                .ForMember(g => g.GradeId, opt => opt.Ignore());

            CreateMap<RoleResource, ApplicationRole>()
                .ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<EnrollmentResource, Enrollment>()
                .ForMember(e => e.EnrollmentId, opt => opt.Ignore());

            CreateMap<GroupResource, Group>()
                .ForMember(g => g.GroupId, opt => opt.Ignore())
                .ForMember(g => g.Enrollments, opt => opt.Ignore())
                .ForMember(g => g.Project, opt => opt.Ignore())
                .ForMember(g => g.Council, opt => opt.Ignore())
                .ForMember(g => g.Lecturer, opt => opt.Ignore())
                .ForMember(g => g.GroupId, opt => opt.Ignore());

            CreateMap<ProjectResource, Project>()
                 .ForMember(p => p.ProjectId, opt => opt.Ignore());

            CreateMap<LecturerResource, Lecturer>()
                 .ForMember(l => l.LecturerId, opt => opt.Ignore());

            CreateMap<MajorResource, Major>()
                 .ForMember(m => m.MajorId, opt => opt.Ignore());

            CreateMap<QuarterResource, Quarter>()
                 .ForMember(m => m.QuarterId, opt => opt.Ignore());
            CreateMap<ExcelResource, Excel>()
                .ForMember(m => m.ExcelId, opt => opt.Ignore());

            CreateMap<CouncilEnrollmentResource, CouncilEnrollment>()
                .ForMember(m => m.CouncilEnrollmentId, opt => opt.Ignore());

            CreateMap<UploadedFileResource, UploadedFile>()
                .ForMember(m => m.UploadedFileId, opt => opt.Ignore());
        }
    }
}