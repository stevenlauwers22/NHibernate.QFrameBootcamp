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
			if (i >= 5)
			{
				var screams = new[]
				{
					"Infrastructure/DontHurtMe/Sounds/ahhh.wav",
					"Infrastructure/DontHurtMe/Sounds/cri-d-effroi-scream.wav",
					"Infrastructure/DontHurtMe/Sounds/ciglik3.wav",
					"Infrastructure/DontHurtMe/Sounds/2scream.wav",
					"Infrastructure/DontHurtMe/Sounds/scream13.wav",
					"Infrastructure/DontHurtMe/Sounds/scream4.wav"
				};

				var soundPlayer = new SoundPlayer(screams[i % screams.Length]);
				soundPlayer.PlaySync();
			}

			return base.OnPrepareStatement(sql);
		}
	}
}