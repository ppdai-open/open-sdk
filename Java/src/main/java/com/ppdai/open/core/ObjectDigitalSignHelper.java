package com.ppdai.open.core;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Created by xuzhishen on 2016/3/11.
 */
public class ObjectDigitalSignHelper {

    /**
     * 获取待签名对象的排序字符串
     * @param propertyObjects
     * @return
     */
    public static String getObjectHashString(PropertyObject ...propertyObjects){
        List<String> list = new ArrayList<String>();
        for (PropertyObject propertyObject : propertyObjects){
            if (propertyObject.isSign())
                list.add(propertyObject.getLowerName());
        }
        Collections.sort(list);

        StringBuffer sb = new StringBuffer();
        for (String ln : list){
            for (PropertyObject propertyObject : propertyObjects){
                if (ln.equals(propertyObject.getLowerName())){
                    sb.append(propertyObject.toString());
                    break;
                }
            }
        }
        return sb.toString();
    }
}
