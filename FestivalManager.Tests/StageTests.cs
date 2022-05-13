// Use this file for your unit tests.
// When you are ready to submit, REMOVE all using statements to Festival Manager (entities/controllers/etc)
// Test ONLY the Stage class. 


using FestivalManager.Entities;

namespace FestivalManager.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
	public class StageTests
	{
		private Stage stage = null;

		[SetUp]
	    public void InitTest()
	    {
		    stage = new Stage();
	    }

		[Test]
	    public void AddPerformerCannotBeNull()
	    {
			Assert.Throws<ArgumentNullException>(() => stage.AddPerformer(null));
	    }

	    [Test]
	    public void AddPerformerCannotBeLessThanAge18()
	    {
		    Performer performer = new Performer("1", "2", 10);
		    Assert.Throws<ArgumentException>(() => stage.AddPerformer(performer));
	    }

	    [Test]
	    public void AddPerformerSuccessfully()
	    {
		    Performer performer = new Performer("1", "2", 20);
			stage.AddPerformer(performer);
		    Assert.That(stage.Performers.Count, Is.EqualTo (1));
			//Assert.That(stage.Performers.FirstOrDefault().Equals(performer));
	    }

	    [Test]
	    public void AddSongCannotBeNull()
	    {
		    Assert.Throws<ArgumentNullException>(() => stage.AddSong(null));
	    }
		
	    [Test]
	    public void AddSongMustBeMoreThanMinute()
	    {
		    Song song = new Song("name", new TimeSpan(0,0,30));
		    Assert.Throws<ArgumentException>(() => stage.AddSong(song));
	    }

	    [Test]
	    public void AddSongSuccessfully()
	    {
		    Song song = new Song("name", new TimeSpan(0,3,30));
			stage.AddSong(song);
		    Assert.That(song.Name == "name");
	    }

	    [Test]
	    public void AddSongToPerformerSuccessfully()
	    {
		    Performer performer = new Performer("1", "2", 20);
		    Song song = new Song("name", new TimeSpan(0,3,30));
			performer.SongList.Add(song);
			stage.AddPerformer(performer);
			stage.AddSong(song);
			stage.AddSongToPerformer("name", "1 2");
		    Assert.That(performer.SongList.Count > 0);
			//Assert.That(performer.SongList.Contains(song));
	    }

	    [Test]
	    public void AddSongToPerformerNotNullSong()
	    {

		    Assert.Throws<ArgumentNullException>(() => stage.AddSongToPerformer(null, "name"));
		    Assert.Throws<ArgumentNullException>(() => stage.AddSongToPerformer("name", null));
	    }

	    [Test]
	    public void PlayIsSuccessfully()
	    {
		    Performer performer = new Performer("Pesho", "Petrov", 20);
		    Song song = new Song("Vetrove", new TimeSpan(0,3,30));
			stage.AddPerformer(performer);
			stage.AddSong(song);
			stage.AddSongToPerformer("Vetrove", "Pesho Petrov");
			string result = stage.Play();

		    Assert.That(result == "1 performers played 1 songs");
	    }

	    [Test]
	    public void GetPerformerExceptionNoSong()
	    {
		    Performer performer = new Performer("Pesho", "Petrov", 20);
		    stage.AddPerformer(performer);

		    Assert.Throws<ArgumentException>(() => stage.AddSongToPerformer("Vetrove", "Pesho Petrov"));
	    }

	    [Test]
	    public void GetPerformerExceptionNoPerformer()
	    {
		    Song song = new Song("Vetrove", new TimeSpan(0,3,30));
		    stage.AddSong(song);

		    Assert.Throws<ArgumentException>(() => stage.AddSongToPerformer("Vetrove", "Pesho Petrov"));
	    }
	}
}