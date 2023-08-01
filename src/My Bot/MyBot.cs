using ChessChallenge.API;
using System;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        return GetBestMove(board, 5);
    }

    private Move GetBestMove(Board board, int depth)
    {

        Move[] moves = board.GetLegalMoves();
        var best = (score: double.MinValue, index: -1);
        for (int i = 0; i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            var newScore = Minimax(board, depth, false, double.MinValue, double.MaxValue);
            if (newScore > best.score)
            {
                best.score = newScore;
                best.index = i;
            }
            board.UndoMove(moves[i]);
        }
        return moves[best.index];
    }

    private double Minimax(Board board, int depth, bool maximizer, double alpha, double beta)
    {
        if (depth == 0)
        {
            return EvaluateMaterial(board);
        }
        Move[] moves = board.GetLegalMoves();
        var best = (score: 0.0, index: 0);
        if (maximizer)
        {
            best = (score: double.MinValue, index: -1);
            for (int i = 0; i < moves.Length; i++)
            {
                board.MakeMove(moves[i]);
                double newVal = Minimax(board, depth - 1, !maximizer, alpha, beta);
                board.UndoMove(moves[i]);
                if (newVal > best.score)
                {
                    best.score = newVal;
                    best.index = i;
                }
                alpha = Math.Max(alpha, best.score);
                if (beta <= alpha) {
                    break;
                }
                
            }
        }
        else
        {
            best = (score: double.MaxValue, index: -1);
            for (int i = 0; i < moves.Length; i++)
            {
                board.MakeMove(moves[i]);
                double newVal = Minimax(board, depth - 1, !maximizer, alpha, beta);
                board.UndoMove(moves[i]);
                if (newVal < best.score)
                {
                    best.score = newVal;
                    best.index = i;
                }
                beta = Math.Min(beta, best.score);
                if (beta <= alpha) {
                    break;
                }
            }
        }
        return best.score;
    }

    private double EvaluateMaterial(Board board)
    {

        var white_pawns_n = board.GetPieceList(PieceType.Pawn, true).Count;
        var white_knights_n = board.GetPieceList(PieceType.Knight, true).Count;
        var white_bishop_n = board.GetPieceList(PieceType.Bishop, true).Count;
        var white_rook_n = board.GetPieceList(PieceType.Rook, true).Count;
        var white_queen_n = board.GetPieceList(PieceType.Queen, true).Count;

        var black_pawns_n = board.GetPieceList(PieceType.Pawn, false).Count;
        var black_knights_n = board.GetPieceList(PieceType.Knight, false).Count;
        var black_bishop_n = board.GetPieceList(PieceType.Bishop, false).Count;
        var black_rook_n = board.GetPieceList(PieceType.Rook, false).Count;
        var black_queen_n = board.GetPieceList(PieceType.Queen, false).Count;

        double pawn_val = 1;
        double knight_val = 3;
        double bishop_val = 3;
        double rook_val = 5;
        double queen_val = 9;

        double white_material = pawn_val * white_pawns_n + knight_val * white_knights_n + bishop_val * white_bishop_n + rook_val * white_rook_n + white_queen_n * queen_val;
        double black_material = pawn_val * black_pawns_n + knight_val * black_knights_n + bishop_val * black_bishop_n + rook_val * black_rook_n + black_queen_n * queen_val;
        if (board.IsWhiteToMove)
        {
            return white_material - black_material;
        }
        return black_material - white_material;
    }
}