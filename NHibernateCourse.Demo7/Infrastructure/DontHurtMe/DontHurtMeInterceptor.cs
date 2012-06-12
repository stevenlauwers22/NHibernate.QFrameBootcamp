using System.Media;
using NHibernate;
using NHibernate.SqlCommand;

namespace NHibernateCourse.Demo7.Infrastructure.DontHurtMe
{
	public class DontHurtMeInterceptor : EmptyInterceptor
	{
	    private static int _numberOfRequest;

		public override SqlString OnPrepareStatement(SqlString sql)
		{
            var i = _numberOfRequest += 1;
			if (i >= 8)
			{
				var screams = new[]
				{
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/ahhh.wav",
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/cri-d-effroi-scream.wav",
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/ciglik3.wav",
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/2scream.wav",
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/scream13.wav",
					"http://www.shockwave-sound.com/sound-effects/scream-sounds/scream4.wav"
				};

				var soundPlayer = new SoundPlayer(screams[i % screams.Length]);
				soundPlayer.PlaySync();
			}

			return base.OnPrepareStatement(sql);
		}
	}
}