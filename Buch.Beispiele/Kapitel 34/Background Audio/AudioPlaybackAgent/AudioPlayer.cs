using System;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using System.Collections.Generic;

namespace AudioPlaybackAgent
{
    public class AudioPlayer : AudioPlayerAgent
    {
        // Aktuelle Titel-Nummer
        private static int currentTrackNumber = 0;
        
        // Titel, die abgespielt werden sollen
        private static List<AudioTrack> _playList = new List<AudioTrack>
        {
            new AudioTrack(new Uri("Enemy_at_the_gates.mp3", UriKind.Relative), "Enemy at the gates", "Celestial Aeon Project", "Miracle", null),
            new AudioTrack(new Uri("Requiem.mp3", UriKind.Relative), "Requiem", "Hiddeminside", "Time to Chill vol.2", null),
            new AudioTrack(new Uri("Sunday_Walk.mp3", UriKind.Relative), "Sunday Walk", "Victoria Caffè", "Busy Days", null),
            new AudioTrack(new Uri("http://traffic.libsyn.com/wpradio/WPRadio_29.mp3", UriKind.Absolute), "Episode 29", "Windows Phone Radio", "Windows Phone Radio Podcast", null)
        };

        /// <remarks>
        /// AudioPlayer instances can share the same process. 
        /// Static fields can be used to share state between AudioPlayer instances
        /// or to communicate with the Audio Streaming agent.
        /// </remarks>
        public AudioPlayer()
        {

        }

        /// <summary>
        /// Called when the playstate changes, except for the Error state (see OnError)
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time the playstate changed</param>
        /// <param name="playState">The new playstate of the player</param>
        /// <remarks>
        /// Play State changes cannot be cancelled. They are raised even if the application
        /// caused the state change itself, assuming the application has opted-in to the callback.
        /// 
        /// Notable playstate events: 
        /// (a) TrackEnded: invoked when the player has no current track. The agent can set the next track.
        /// (b) TrackReady: an audio track has been set and it is now ready for playack.
        /// 
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
        {
            switch (playState)
            {
                case PlayState.TrackReady:
                    // Wiedergabe wurde in PlayTrack() bereits festgelegt
                    player.Play();
                    break;
                case PlayState.TrackEnded:
                    PlayNextTrack(player);
                    break;
            }

            NotifyComplete();
        }

        /// <summary>
        /// Called when the user requests an action using application/system provided UI
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time of the user action</param>
        /// <param name="action">The action the user has requested</param>
        /// <param name="param">The data associated with the requested action.
        /// In the current version this parameter is only for use with the Seek action,
        /// to indicate the requested position of an audio track</param>
        /// <remarks>
        /// User actions do not automatically make any changes in system state; the agent is responsible
        /// for carrying out the user actions if they are supported.
        /// 
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
        {
            switch (action)
            {
                case UserAction.Play:
                    PlayTrack(player);
                    break;
                case UserAction.Pause:
                    player.Pause();
                    break;
                case UserAction.SkipPrevious:
                    PlayPreviousTrack(player);
                    break;
                case UserAction.SkipNext:
                    PlayNextTrack(player);
                    break;
            }
            
            NotifyComplete();
        }

        private void PlayNextTrack(BackgroundAudioPlayer player)
        {
            if (++currentTrackNumber >= _playList.Count)
            {
                currentTrackNumber = 0;
            }

            PlayTrack(player);
        }

        private void PlayPreviousTrack(BackgroundAudioPlayer player)
        {
            if (--currentTrackNumber < 0)
            {
                currentTrackNumber = _playList.Count - 1;
            }

            PlayTrack(player);
        }

        private void PlayTrack(BackgroundAudioPlayer player)
        {
            player.Track = _playList[currentTrackNumber];
        }
        
        /// <summary>
        /// Called whenever there is an error with playback, such as an AudioTrack not downloading correctly
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track that had the error</param>
        /// <param name="error">The error that occured</param>
        /// <param name="isFatal">If true, playback cannot continue and playback of the track will stop</param>
        /// <remarks>
        /// This method is not guaranteed to be called in all cases. For example, if the background agent 
        /// itself has an unhandled exception, it won't get called back to handle its own errors.
        /// </remarks>
        protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
        {
            if (isFatal)
            {
                Abort();
            }
            else
            {
                NotifyComplete();
            }

        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// </summary>
        /// <remarks>
        /// Once the request is Cancelled, the agent gets 5 seconds to finish its work,
        /// by calling NotifyComplete()/Abort().
        /// </remarks>
        protected override void OnCancel()
        {
            NotifyComplete();
        }
    }
}
