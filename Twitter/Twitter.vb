Imports LinqToTwitter

Public Class Twitter

    Private twitterContext As TwitterContext

    Public Sub New(ConsumerKey As String, ConsumerSecret As String, OAuthToken As String, AccessToken As String)

        ' create twitter context if it doesn't exist
        If IsNothing(twitterContext) Then

            ' set up credentials to initialize context with
            Dim creds = New SingleUserAuthorizer With {.Credentials = New InMemoryCredentials With {.ConsumerKey = ConsumerKey, _
                                                                                                    .ConsumerSecret = ConsumerSecret, _
                                                                                                    .OAuthToken = OAuthToken, _
                                                                                                    .AccessToken = AccessToken}}
            twitterContext = New TwitterContext(creds)
        End If

    End Sub

    Public Function GetTweetsByUser(userName As String) As List(Of Tweet)

        Try

            ' linqtotwitter query to get 'statuses' (tweets) by username
            Dim tweets = (From tweet In twitterContext.Status _
                          Where tweet.Type = StatusType.User And tweet.ScreenName = userName _
                          Select tweet).ToList

            ' create list of tweet objects to return
            Dim tweetList As New List(Of Tweet)

            ' take required info from tweets
            For Each t In tweets
                tweetList.Add(New Tweet With {.Name = t.ScreenName, _
                                              .Text = t.Text, _
                                              .CreatedAt = t.CreatedAt})
            Next

            'return list
            Return tweetList

        Catch ex As Exception

            Throw New Exception("Could not retrieve tweets", ex)

        End Try


    End Function

End Class