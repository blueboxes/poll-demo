import styled from "styled-components";
import { useState } from 'react'
import ResultsPanel from './components/ResultsPanel';
import VotePanel from './components/VotePanel'
import { PollQuestion, PollResult } from "./types";
import * as signalR from "@microsoft/signalr";
  
export default function App() {
  const [pollQuestion, setPollQuestion] = useState<PollQuestion | null>();
  const [pollResults, setPollResults] = useState<PollResult[] | []>();

  const voted = async (question : PollQuestion, results: PollResult[]) => {
    setPollResults(results);
    setPollQuestion(question);

    //To get live updates lets use signalR rather than polling
    await connect(question);
  }

  const subscribe = async (data: PollQuestion, connection : signalR.HubConnection) => {

    //join group for this question
    await window.fetch(`${import.meta.env.VITE_API_ROOT}/api/polls/${data.questionId}/subscribe`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ id: data.questionId, connectionId: connection.connectionId }),
    });
    
    //subscribe to new results
    connection?.on("new-results", (newResults: PollResult[]) => {
     setPollResults(newResults);
    });
  }

  const connect = async (data : PollQuestion) => {
    
    const apiBaseUrl = `${import.meta.env.VITE_API_ROOT}/api`;
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(apiBaseUrl)
      .withAutomaticReconnect()
      .build();
 
    //This method is called to create the connection
    //to SignalR so the client can receive messages
    connection
      .start()
      .then(async () => {
        await subscribe(data, connection);        
      })
      .catch(function (err) {
        return console.error(err.toString());
      });    
  }

  return (
    <>
       {pollResults ? (
        <ResultsPanel question={pollQuestion} results={pollResults} />
      ) : (
          <VotePanel onVotedCallBack={voted} />
      )}
    </>
  );
}
