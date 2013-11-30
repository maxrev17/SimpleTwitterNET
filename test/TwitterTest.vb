Imports SimpleTwitter

Module TwitterTest

    Sub Main()

        ' create twitter object with credentials (available from dev.twitter.com)
        Dim twitter = New SimpleTwitter.Twitter("", _
                                                         "", _
                                                         "", _
                                                         "")


        ' iterate over the tweet objects outputting to console
        For Each twit As SimpleTwitter.Tweet In twitter.GetTweetsByUser("newsycombinator")
            Console.WriteLine(twit.Name)
            Console.WriteLine(twit.CreatedAt)
            Console.WriteLine(twit.Text)

        Next

        ' hold console open
        Console.Read()

    End Sub

End Module
