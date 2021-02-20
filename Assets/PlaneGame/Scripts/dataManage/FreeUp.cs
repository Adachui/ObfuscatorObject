using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FreeUp{

    private static int[] upLvArray = {1,2,2,2,3,4};
    private static float[] rateArray = { 0.15f, 0.15f, 0.3f, 0.3f, 0.5f, 0.5f };


    public static int GetFreeUpLv(int buildLv,int payType)
    {
        int upLv = 0;
        if(payType == 0)
        {
            int subLv = buildLv - DataManager.maxBuildByCoinLv;
            if (subLv >= 0 && subLv <= 5){
                int rateNUm = (int)(rateArray[subLv] * 100);
                int randomNum = Random.Range(1, 100);
                if(randomNum <= rateNUm)
                    upLv = upLvArray[subLv];
            }
        }
        return upLv;
    }
}
