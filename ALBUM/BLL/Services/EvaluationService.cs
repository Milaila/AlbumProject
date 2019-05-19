using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using BLL.DTO;
using BLL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IAlbumUnitOfWork _db;
        private readonly IMapper _mapper;
        private bool _isDisposed = false;

        public EvaluationService(IAlbumUnitOfWork db, MapperConfiguration mapperConfig)
        {
            _db = db
                ?? throw new ArgumentNullException("UnitOfWork must be not null!");
            _mapper = mapperConfig?.CreateMapper()
                ?? throw new ArgumentNullException("Mapper configuration must be not null!");
        }

        public int MakeEvaluation(EvaluationDTO evaluation)
        {
            ValidateEvaluation(evaluation);
            if (GetDALEvaluationByImageAndProfile(evaluation.ImageId, evaluation.ProfileId) != null)
                throw new ServiceException(ExceptionType.UniqueException, 
                    $"Profile [{evaluation.ProfileId}] already appreciated the image [{evaluation.ImageId}]!");
            Evaluation evaluationDAL = _db.EvaluationRepository
                .Add(_mapper.Map<Evaluation>(evaluation));
            _db.Save();
            return evaluationDAL.Id;
        }

        public bool DeleteEvaluationById(int evaluationId)
        {
            if (!_db.EvaluationRepository.Contains(evaluationId))
                return false;
            _db.EvaluationRepository.Delete(evaluationId);
            _db.Save();
            return true;
        }

        public int UpdateOrCreateEvaluation(EvaluationDTO evaluation)
        {
            if (evaluation == null)
                throw new ServiceException("Evaluation is null!")
                {
                    Type = ExceptionType.NullException
                };
            Evaluation evaluationDAL = _db.EvaluationRepository.Get(evaluation.Id) ??
                GetDALEvaluationByImageAndProfile(evaluation.ImageId, evaluation.ProfileId);
            if (evaluationDAL == null)
                return MakeEvaluation(evaluation);
            ValidateEvaluation(evaluation);
            evaluationDAL.Mark = evaluation.Mark;
            _db.Save();
            return evaluationDAL.Id;
        }

        public EvaluationDTO GetEvaluationById(int evaluationId)
        {
            return _mapper.Map<EvaluationDTO>(_db.EvaluationRepository.Get(evaluationId));
        }

        public HashSet<EvaluationDTO> GetEvaluationsByImage(int imageId)
        {
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException
                    (ExceptionType.NotFoundException, $"Image with id = {imageId} was not found!");
            return _mapper.Map<IEnumerable<Evaluation>, HashSet<EvaluationDTO>>(image.Rating);
        }

        public EvaluationDTO GetEvaluationByImageAndProfile(int imageId, int profileId)
        {
            return _mapper.Map<EvaluationDTO>
                (GetDALEvaluationByImageAndProfile(imageId, profileId));
        }

        void IDisposable.Dispose()
        {
            if (!_isDisposed)
            {
                (_db as IDisposable)?.Dispose();
                _isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }

        private Evaluation GetDALEvaluationByImageAndProfile(int imageId, int profileId)
        {
            if (!_db.ImageRepository.Contains(imageId))
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Image with id = {imageId} was not found!");
            if (!_db.ProfileRepository.Contains(profileId))
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            return _db.EvaluationRepository
                .GetAll()
                .Where(x => (x.ImageId == imageId) && (x.ProfileId == profileId))
                .FirstOrDefault();
        }

        private void ValidateEvaluation(EvaluationDTO evaluation)
        {
            if (evaluation == null)
                throw new ServiceException("Evaluation is null")
                {
                    Type = ExceptionType.NullException
                };
            if ((evaluation.Mark > EvaluationDTO.MaxMark)
                    || (evaluation.Mark < EvaluationDTO.MinMark))
                throw new ServiceException(ExceptionType.InvalidFieldException,
                    $"Invalid mark: {evaluation.Mark}. It must be in range from 1 to 10.");
            if (!_db.ProfileRepository.Contains(evaluation.ProfileId))
                throw new ServiceException($"Profile with id = {evaluation.ProfileId} was not found!")
                {
                    Type = ExceptionType.ForeignKeyException,
                };
            if (!_db.ImageRepository.Contains(evaluation.ImageId))
                throw new ServiceException($"Image with id = {evaluation.ImageId} was not found!")
                {
                    Type = ExceptionType.ForeignKeyException,
                };
        }
    }
}
