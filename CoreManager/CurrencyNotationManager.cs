using System;
using UnityEngine;

public static class CurrencyNotationManager
{
    private const string Zero = "0";

    private static readonly string[] CurrencyUnits = new string[] { "", "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
    "aa","ab","ac","ad","ae","af","ag","ah","ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
    "ba","bb","bc","bd","be","bf","bg","bh","bi","bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz",
    "ca","cb","cc","cd","ce","cf","cg","ch","ci","cj","ck","cl","cm","cn","co","cp","cq","cr","cs","ct","cu","cv","cw","cx","cy","cz",
    "da","db","dc","dd","de","df","dg","dh","di","dj","dk","dl","dm","dn","do","dp","dq","dr","ds","dt","du","dv","dw","dx","dy","dz",
    "ea","eb","ec","ed","ee","ef","eg","eh","ei","ej","ek","el","em","en","eo","ep","eq","er","es","et","eu","ev","ew","ex","ey","ez",
    "fa","fb","fc","fd","fe","ff","fg","fh","fi","fj","fk","fl","fm","fn","fo","fp","fq","fr","fs","ft","fu","fv","fw","fx","fy","fz"};

    // Start is called before the first frame update

    public static string ToCurrencyString(this double number)
    {
        if (-1d < number && number < 1d)
            return Zero;
        if (true == double.IsInfinity(number))
            return "Infinity";

        //부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;
        //보여줄 숫자
        string showNumber = string.Empty;
        //단위 문자열
        string unitString = string.Empty;
        //패턴을 단순화 시키기 위해 무조건 지수표현식으로 변경한 후 처리한다
        string[] partsSplit = number.ToString("E").Split('+');

        if (partsSplit.Length < 2)
        {
            Debug.Log(string.Format("Failed - ToCurrencyString({0})", number));
            return Zero;
        }

        //지수(자릿수 표현)
        if (false == int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.Log(string.Format("Failed - ToCurrencyString({0}) : partsSplit[1] = {1}", number, partsSplit[1]));
            return Zero;
        }

        //몫은 문자열 인덱스
        int quotient = exponent / 3;

        //나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //1a미만은 그냥 표현
        if (exponent < 3)
            showNumber = Math.Truncate(number).ToString();
        else
        {
            //10의 거듭제곱을 구해서 자릿수 표현값을 만들어준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * Math.Pow(10, remainder);

            //소수 둘째 자리까지만 출력
            showNumber = temp.ToString("F").Replace(".000", "");
        }

        unitString = CurrencyUnits[quotient];
        return string.Format("{0}{1}{2}", significant, showNumber, unitString);
    }
}