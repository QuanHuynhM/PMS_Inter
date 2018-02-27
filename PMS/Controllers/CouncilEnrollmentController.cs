﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PMS.Persistence.IRepository;
using PMS.Persistence;
using PMS.Resources;
using PMS.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PMS.Controllers
{
    [Route("/api/boardenrollments")]
    public class boardEnrollmentController : Controller
    {
        private IMapper mapper;
        private IBoardEnrollmentRepository boardEnrollmentRepository;
        private ILecturerRepository lecturerRepository;
        private IBoardRepository boardRepository;
        private IUnitOfWork unitOfWork;

        public boardEnrollmentController(IMapper mapper, IUnitOfWork unitOfWork,
            IBoardEnrollmentRepository boardEnrollmentRepository,
            ILecturerRepository lecturerRepository, IBoardRepository boardRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.boardEnrollmentRepository = boardEnrollmentRepository;
            this.lecturerRepository = lecturerRepository;
            this.boardRepository = boardRepository;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateboardEnrollment([FromBody]BoardEnrollmentResource boardEnrollmentResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boardEnrollment = mapper.Map<BoardEnrollmentResource, BoardEnrollment>(boardEnrollmentResource);
            boardEnrollment.Lecturer = await lecturerRepository.GetLecturer(boardEnrollmentResource.LecturerID);
            boardEnrollment.Board = await boardRepository.GetBoard(boardEnrollmentResource.BoardID);
            if (boardEnrollment.Score != null)
            {
                boardEnrollment.isMarked = true;
            }

            boardEnrollmentRepository.AddBoardEnrollment(boardEnrollment);
            await unitOfWork.Complete();

            boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollment(boardEnrollment.BoardEnrollmentId);

            var result = mapper.Map<BoardEnrollment, BoardEnrollmentResource>(boardEnrollment);

            return Ok(result);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateboardEnrollment(int id, [FromBody]BoardEnrollmentResource boardEnrollmentResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollment(id);

            if (boardEnrollment == null)
                return NotFound();

            mapper.Map<BoardEnrollmentResource, BoardEnrollment>(boardEnrollmentResource, boardEnrollment);
            boardEnrollment.Lecturer = await lecturerRepository.GetLecturer(boardEnrollmentResource.LecturerID);
            boardEnrollment.Board = await boardRepository.GetBoard(boardEnrollmentResource.BoardID);
            if (boardEnrollment.Score != null)
            {
                boardEnrollment.isMarked = true;
            }
            await unitOfWork.Complete();

            var result = mapper.Map<BoardEnrollment, BoardEnrollmentResource>(boardEnrollment);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteboardEnrollment(int id)
        {
            var boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollment(id, includeRelated: false);

            if (boardEnrollment == null)
            {
                return NotFound();
            }

            boardEnrollmentRepository.RemoveBoardEnrollment(boardEnrollment);
            await unitOfWork.Complete();

            return Ok(id);
        }

        [HttpGet]
        [Route("getboardenrollment/{id}")]
        public async Task<IActionResult> GetboardEnrollment(int id)
        {
            var boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollment(id);

            if (boardEnrollment == null)
            {
                return NotFound();
            }

            var boardEnrollmentResource = mapper.Map<BoardEnrollment, BoardEnrollmentResource>(boardEnrollment);

            return Ok(boardEnrollmentResource);
        }

        [HttpGet]
        [Route("getboardenrollmentbylectureremail/{email}")]
        public async Task<IActionResult> GetboardEnrollmentByLecturerEmail(string email, [FromBody]BoardResource boardResource)
        {
            var boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollmentByLecturerEmail(email, boardResource);
            if (boardEnrollment == null)
            {
                return NotFound();
            }
            var boardEnrollmentResource = mapper.Map<BoardEnrollment, BoardEnrollmentResource>(boardEnrollment);

            return Ok(boardEnrollmentResource);
        }

        [HttpGet]
        [Route("getall")]
        public async Task<QueryResultResource<BoardEnrollmentResource>> GetboardEnrollments(QueryResource queryResource)
        {
            var query = mapper.Map<QueryResource, Query>(queryResource);

            var queryResult = await boardEnrollmentRepository.GetBoardEnrollments(query);

            var a = mapper.Map<QueryResult<BoardEnrollment>, QueryResultResource<BoardEnrollmentResource>>(queryResult);

            foreach (var item in a.Items)
            {
                item.Board = mapper.Map<Board, BoardResource>(await boardRepository.GetBoard(item.BoardID));
            }

            return a;
        }

        [HttpGet]
        [Route("getboardenrollmentsbylectureremail/{email}")]
        public async Task<QueryResultResource<BoardEnrollmentResource>> GetboardEnrollments(string email)
        {

            var queryResult = await boardEnrollmentRepository.GetBoardEnrollmentsByLecturerEmail(email);
            return mapper.Map<QueryResult<BoardEnrollment>, QueryResultResource<BoardEnrollmentResource>>(queryResult);
        }

        [HttpGet]
        [Route("getboardenrollmentsbyboardid/{id}")]
        public async Task<IActionResult> GetboardEnrollmentsByboardId(int id)
        {
            var boardEnrollments = await boardEnrollmentRepository.GetBoardEnrollmentsByBoardId(id);
            var boardEnrollmentResource = mapper.Map<IEnumerable<BoardEnrollment>, IEnumerable<BoardEnrollmentResource>>(boardEnrollments);
            return Ok(boardEnrollmentResource);
        }

        [HttpPut]
        [Route("savescore/{id}")]
        public async Task<IActionResult> SaveScore(int id, int? score)
        {
            var boardEnrollment = await boardEnrollmentRepository.GetBoardEnrollment(id);

            boardEnrollment.Score = score;
            boardEnrollment.isMarked = true;
            boardEnrollmentRepository.UpdateScore(boardEnrollment);
            await unitOfWork.Complete();

            var boardEnrollmentResource = mapper.Map<BoardEnrollment, BoardEnrollmentResource>(boardEnrollment);
            return Ok(boardEnrollmentResource);
        }
    }
}
