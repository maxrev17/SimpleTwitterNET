Imports LinqToTwitter
Imports System.Text.RegularExpressions

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

                t.Text = AddWebAndTwitterLinks(t.Text)

                tweetList.Add(New Tweet With {.Name = t.ScreenName, _
                                              .Text = t.Text, _
                                              .CreatedAt = t.CreatedAt})
            Next

            'return list of tweets
            Return tweetList

        Catch ex As Exception

            Throw New Exception("Could not retrieve tweets", ex)

        End Try


    End Function

    Private Function AddWebAndTwitterLinks(input As String) As [String]
        Dim strWebLinks As String = "(^|[\n ])([\w]+?://[\w]+[^ \n\r\t< ]*)"
        Dim strWebLinksWWW As String = "(^|[\n ])((www|ftp)\.[^ \t\n\r< ]*)"
        Dim strTwitterNames As String = "@(\w+)"
        Dim strTwitterTags As String = "#(\w+)"
        input = Regex.Replace(input, strWebLinks, " <a href=""$2"" target=""_blank"">$2</a>")
        input = Regex.Replace(input, strWebLinksWWW, " <a href=""http://$2"" target=""_blank"">$2</a>")
        input = Regex.Replace(input, strTwitterNames, "<a href=""http://www.twitter.com/$1"" target=""_blank"">@$1</a>")
        input = Regex.Replace(input, strTwitterTags, "<a href=""https://twitter.com/search?q=%23$1" + "&src=hash"" target=""_blank"">#$1</a>")

        Return input
    End Function


End Class