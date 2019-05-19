using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IEvaluationService : IDisposable
    {
        int MakeEvaluation(EvaluationDTO evaluation);
        EvaluationDTO GetEvaluationById(int evaluationId);
        HashSet<EvaluationDTO> GetEvaluationsByImage(int imageId);
        int UpdateOrCreateEvaluation(EvaluationDTO evaluation);
        EvaluationDTO GetEvaluationByImageAndProfile(int imageId, int profileId);
        bool DeleteEvaluationById(int evaluationId);
    }
}
