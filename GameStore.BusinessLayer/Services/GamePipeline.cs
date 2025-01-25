using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Services
{
    public class GamePipeline
    {
        private readonly List<IPipelineStep> _steps;

        public GamePipeline()
        {
            _steps = new List<IPipelineStep>();
        }

        public GamePipeline AddStep(IPipelineStep step)
        {
            _steps.Add(step);
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            foreach (var step in _steps)
            {
                input = step.Execute(input, queryDto);
            }

            return input;
        }
    }
}
