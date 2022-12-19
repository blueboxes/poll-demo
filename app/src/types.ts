export interface PollOption {
    prompt: string;
    answerId: number;
}

export interface PollResult {
    answerId: number;
    percentage: number;
}

export interface PollQuestion {
    questionId: string;
    questionText: string;
    options: Array<PollOption>;
    results: Array<PollResult>;
}