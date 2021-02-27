using Extensions;

using PlanetGeneration;

using Rendering;

using UnityEngine;

namespace Behaviours.Planet
{
    public class PlanetGenerationTest : MonoBehaviour
    {
        public GameObject player;
        
        private PlanetController _planetController;
        public void Start()
        {
            var settings = new PlanetGeneratorSettings
            {
                HeightLimit = 5,
                BlockHeight = 3,
            };
            
            var planet = new PlanetGenerator(settings).Generate();

            _planetController = new PlanetController(planet, player);
        }

        public void Update()
        {
            _planetController.UpdateRenderedRegions();
        }
    }
}
