using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCoral
{
    public class GameScreen
    {

        public ProjectCoralGame _game;
        public ProjectCoralGame Game {get { return _game; }}

        public GameScreen(ProjectCoralGame game)
        {
            this._game = game;
        }

        public virtual void Initialize() {}
        public virtual void LoadContent() {}
        public virtual void Activate() {}
        public virtual void Deactivate() {}
        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime) {}
        public virtual void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch) {}

    }
}
