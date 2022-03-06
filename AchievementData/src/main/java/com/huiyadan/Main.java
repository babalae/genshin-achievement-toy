/**
 * Copyright © 2004-2017 LianlianPay.All Rights Reserved.
 */
package com.huiyadan;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.huiyadan.vo.ExcelVo;
import com.huiyadan.vo.TargetVo;
import io.github.biezhi.excel.plus.Reader;
import io.github.biezhi.excel.plus.Writer;
import org.apache.commons.io.FileUtils;
import org.apache.commons.lang.StringUtils;

import java.io.File;
import java.io.IOException;
import java.util.*;
import java.util.stream.Collectors;

/**
 * 一个用于生成期望json数据的辅助程序
 */
public class Main {
    static final ObjectMapper objectMapper = new ObjectMapper();

    public static void main(String[] args) throws Exception {
        String jsonPath = "data/paimon.moe.achievement.json";
        String excelPath = "data/原神 586 个成就与 138 个每日委托汇总.xlsx";
        List<TargetVo> listFromJson = importFromJson(jsonPath);
        System.out.println("载入json数据量：" + listFromJson.size());
        List<ExcelVo> excelVoList = importFromExcel(excelPath);
        List<TargetVo> listFromExcel = ExcelVo.toTargetVo(excelVoList);
        System.out.println("载入excel数据量：" + listFromExcel.size());

        // 按特辑分类
        Map<String, List<TargetVo>> map = listFromJson.stream().collect(Collectors.groupingBy(TargetVo::getEdition, Collectors.toList()));
        for (Map.Entry<String, List<TargetVo>> stringListEntry : map.entrySet()) {
            for (TargetVo targetVo : stringListEntry.getValue()) {
                targetVo.setEdition(null);
            }
        }
        System.out.println(objectMapper.writeValueAsString(map));
        //  校验数据
        Iterator<TargetVo> it = listFromExcel.iterator();
        while (it.hasNext()) {
            TargetVo voFromExcel = it.next();
            for (TargetVo voFromJson : listFromJson) {
                if (StringUtils.isBlank(voFromExcel.getId())) {
                    it.remove();
                } else {
                    if (voFromExcel.getId().equals(voFromJson.getId())) {
                        voFromExcel.check(voFromJson);
                        break;
                    }
                }
            }
        }

        // 查看校验结果
//        printCheckResult(listFromExcel);
//        printCheckResult(listFromJson);

//        Writer.create()
//                .withRows(listFromExcel)
//                .to(new File("data/结果11.xlsx"));
//        Writer.create()
//                .withRows(listFromJson)
//                .to(new File("data/结果22.xlsx"));


//        System.out.println(objectMapper.writeValueAsString(listFromJson));
    }

    static void printCheckResult(List<TargetVo> list) throws Exception {
        for (TargetVo vo : list) {
            if (!vo.isChecked()) {
                System.out.println("未校验数据:" + objectMapper.writeValueAsString(vo));
            }
        }
    }

    /**
     * 数据来自
     * https://github.com/MadeBaruna/paimon-moe/blob/main/src/data/achievement/zh.json
     */
    static List<TargetVo> importFromJson(String path) throws Exception {
        List<TargetVo> targetVoList = new ArrayList<>();

        JsonNode node = objectMapper.readValue(new File(path), JsonNode.class);
        Iterator<JsonNode> elements = node.elements();
        while (elements.hasNext()) {
            JsonNode editions = elements.next(); // name + achievements []
            String editionName = editions.path("name").asText();
            Iterator<JsonNode> achievements = editions.path("achievements").elements();
            while (achievements.hasNext()) {
                JsonNode objectOrArray = achievements.next();
                if (objectOrArray.isArray()) {
                    Iterator<JsonNode> obj = objectOrArray.elements();
                    while (obj.hasNext()) {
                        TargetVo vo = buildTargetVo(editionName, obj.next(), objectOrArray);
                        targetVoList.add(vo);
                    }
                } else if (objectOrArray.isObject()) {
                    TargetVo vo = buildTargetVo(editionName, objectOrArray, null);
                    targetVoList.add(vo);
                }
            }
        }
        return targetVoList;
    }

    static TargetVo buildTargetVo(String edition, JsonNode achievement, JsonNode fromArray) {
        TargetVo targetVo = new TargetVo();
        targetVo.setId(String.valueOf(achievement.get("id").asInt()));
        targetVo.setVer(achievement.get("ver").asText());
        targetVo.setEdition(edition);
        targetVo.setName(achievement.get("name").asText());
        targetVo.setDesc(achievement.get("desc").asText());
        targetVo.setReward(String.valueOf(achievement.get("reward").asInt()));
        if (fromArray != null) {
            targetVo.setLevels(new ArrayList<>());
            Iterator<JsonNode> elements = fromArray.elements();
            while (elements.hasNext()) {
                targetVo.getLevels().add(elements.next().get("id").asText());
            }
        }
        return targetVo;
    }

    /**
     * 数据来自
     * https://docs.qq.com/sheet/DS01hbnZwZm5KVnBB?tab=BB08J3
     */
    static List<ExcelVo> importFromExcel(String path) {
        List<ExcelVo> sheet1DataList = Reader.create(ExcelVo.class)
                .from(new File(path))
                .sheet(1)
                .start(2)
                .asList();
        List<ExcelVo> sheet2DataList = Reader.create(ExcelVo.class)
                .from(new File(path))
                .sheet(2)
                .start(1)
                .asList();
        sheet1DataList.addAll(sheet2DataList);
        return sheet1DataList;
    }

}
